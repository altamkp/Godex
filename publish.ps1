function Invoke-Utility {
    # please see https://stackoverflow.com/questions/48864988/powershell-with-git-command-error-handling-automatically-abort-on-non-zero-exi/48877892#48877892
    $exe, $argsForExe = $Args
    $ErrorActionPreference = 'Continue'
    try { & $exe $argsForExe } catch { Throw } # catch is triggered ONLY if $exe can't be found, never for errors reported by $exe itself
    if ($LASTEXITCODE) { Throw "$exe indicated failure (exit code $LASTEXITCODE; full command: $Args)." }
}

$status = $(git status --porcelain)
if (-not ([string]::IsNullOrWhiteSpace($status))) {
    Write-Error "[Error]: Current repo is not clean"
    Exit 1
}

$version = $(git tag --points-at $(git log -n1 --pretty='%h'))
if ($null -eq $version) {
    Write-Error "[Error]: Current commit does not have a version tag, latest version is $(git tag -l --sort=-v:refname | head -1)"

    Exit 1
}
if (-not ($version -match "^v(0|[1-9]+[0-9]*)(\.(0|[1-9]+[0-9]*))+$")) {
    Write-Error "[Error]: Current version $version is not valid. Valid version should be like v1.0.0"
    Exit 1
}

$version = $version.Trim('v')
$product = $(basename $(git rev-parse --show-toplevel))

Invoke-Utility dotnet clean --configuration release
Invoke-Utility dotnet build "-p:Version=$version" --configuration release
Invoke-Utility dotnet pack "-p:Version=$version" --configuration release -o Releases/$version --no-build

Write-Host "[Information]: Publishing $product $version"

# push tag and package
Invoke-Utility git push --tags
Invoke-Utility dotnet nuget push Releases/$version/**.nupkg --source qkabi --api-key api

Write-Host "[Information]: Successfully published $product $version"
