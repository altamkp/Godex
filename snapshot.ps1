function Invoke-Utility {
    # please see https://stackoverflow.com/questions/48864988/powershell-with-git-command-error-handling-automatically-abort-on-non-zero-exi/48877892#48877892
    $exe, $argsForExe = $Args
    $ErrorActionPreference = 'Continue'
    try { & $exe $argsForExe } catch { Throw } # catch is triggered ONLY if $exe can't be found, never for errors reported by $exe itself
    if ($LASTEXITCODE) { Throw "$exe indicated failure (exit code $LASTEXITCODE; full command: $Args)." }
}

$version = $(git describe --tags --abbrev=0)
if ($null -eq $version) {
    Write-Error "[Error]: Current commit does not have a version tag"
    Exit 1
}

$randomSuffix = Get-Random -Minimum 1 -Maximum 10000
$version = $version.Trim('v') + "." + $randomSuffix
# $version = $version.Trim('v')
$product = $(basename $(git rev-parse --show-toplevel))

Invoke-Utility dotnet clean --configuration debug
Invoke-Utility dotnet build "-p:Version=$version" --configuration debug
Invoke-Utility dotnet pack "-p:Version=$version" --configuration debug -o Snapshots/$version --no-build

Write-Host "[Information]: Publishing $product $version"

Invoke-Utility dotnet nuget push Snapshots/$version/**.nupkg --source snapshots

Write-Host "[Information]: Successfully published $product $version"