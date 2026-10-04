# Godex — agent notes

Godot extension libraries for C#, shipped as three NuGet packages. Branches: `master` (default branch for releases and docs deploys).

## Layout

```
Godex.slnx                 # XML solution format
src/Godex/src              # Godex      — base extensions (Microsoft.NET.Sdk + GodotSharp 4.7.2)
src/Godex.Async/src        # Godex.Async — awaitables / CancellableSignalAwaiter
src/Godex.Hosting/src      # Godex.Hosting— Host node + DI (references Godex)
src/<Pkg>/src/README.md    # packed into the NuGet package (PackageReadmeFile)
src/<Pkg>/test             # Godot test project (Godot.NET.Sdk/4.7.2) + project.godot
src/Godex/test/Fixtures/   # .tscn/.cs pairs used by the GDx/PackedScene suites
Directory.Build.props      # <Version> — single source of truth for all three packages
.runsettings-gdunit-ci     # headless runsettings for `dotnet test`
docs/                      # DocFX site sources
```

All three libraries target `net10.0`. `Godex.Async` and `Godex.Hosting` both depend on `Godex`, so `Godex`'s API surface is effectively the base layer — check it before adding anything to the other two.

## Build & verify

```bash
dotnet build Godex.slnx          # only build entry point; there is no lint/format task runner
```

## Testing (gdUnit4)

Tests are [gdUnit4](https://github.com/godot-gdunit-labs/gdUnit4) suites (the C# port,
`gdUnit4.api` + `gdUnit4.test.adapter`) driven through `dotnet test`. There are no test
scenes and no `run/main_scene` any more — a suite is a plain `[TestSuite]` class, and
`.tscn` files survive only as fixtures (`src/Godex/test/Fixtures/`).

`GODOT_BIN` is mandatory. Without it the adapter prints `Godot runtime is not configured`
and then reports `No test is available in <dll>`; that still exits 1, but for the wrong reason.

```bash
# one suite (recommended, and what CI does)
GODOT_BIN=/Applications/Godot_mono.app/Contents/MacOS/Godot \
  dotnet test src/Godex/test/Godex.Tests.csproj --settings .runsettings-gdunit-ci \
  --filter "FullyQualifiedName~Godex.Tests.PoolTest"
```

- Omitting `--settings` auto-discovers `.runsettings` (windowed). `.runsettings-gdunit-ci` is the same file plus `--headless`; it is what CI and scripted runs should use.
- Open projects with **Godot_mono.app** (`godot-mono`), never `Godot.app` — the one on `PATH` is the non-.NET build.
- `[RequireGodotRuntime]` is needed on any suite that touches an engine object. Without it, constructing a `Node` or `InputEvent` **crashes the test host**; the run then prints `Passed!` and `Test Run Aborted`, so it looks green.
- Suites are plain classes, so put **one suite per file**. See "Running suites" below for why.

### Running suites

gdUnit4's VSTest adapter 3.1.1 silently drops results when a whole assembly runs at once, and a dropped result **swallows the failure with it**: with `dotnet test <csproj>` and no filter, `Godex.Tests` discovers 183 tests but reports 173, and a deliberately broken test in that dropped set still reports `Passed!`. Reproducible, not flaky.

So **run one suite per invocation** (`--filter FullyQualifiedName~<Suite>`). Each run then reports every test in that suite, and CI additionally compares the reported count against the adapter's own `Discover: TestSuite <name> with N TestCases found.` line to catch any regression. CI drives this from a matrix built out of those discovery lines, so the list cannot drift.

### Writing assertions

`using static GdUnit4.Assertions` — `AssertInt`/`AssertFloat`/`AssertBool`/`AssertString`/`AssertObject`/`AssertArray`/`AssertVector`/`AssertThrown`. Use `AddNode(node, true)` to attach and `AutoFree(node)` to dispose.

- `AssertVec2`/`AssertVec3` are deprecated; use `AssertVector`.
- A class containing nested `Node` subclasses must be `partial` (GD0002).
- `IVectorAssert.IsEqualApprox(expected, approx)` takes a **vector** tolerance, e.g. `IsEqualApprox(new Vector2(1, 2), new Vector2(1e-4f, 1e-4f))`.
- `-0.5f.IsZeroApprox(p)` parses as `-(bool)`; write `(-0.5f).IsZeroApprox(p)`.
- `Mathf.RoundToInt` rounds halves to the **nearest even** number (`4.5 → 4`, `5.5 → 6`), so integer-vector scaling looks surprising unless tested on purpose.
- `Godot.Timer` quantises by whole process deltas: a `0.05s` timer was observed firing at `0.023s`. Assert loose bounds on short waits, or use ~1s waits.
- `Setting<T>` defers its first setter with `CallDeferred`, so a setting built by one test can still write its config file during the next one. Give each settings test its own scratch path.
- `DirAccess`/`FileAccess` and native objects need a real tree; `QueueFree` is deferred to the end of the frame (assert `IsQueuedForDeletion()`, not `GetChildCount()`).
- The gdUnit4 runner loads the project's autoloads, so `Godex.Hosting` tests get `Host.ServiceProvider` from the existing `ApplicationHost` autoload. Do **not** add a second `Host` — `Host.ServiceProvider` is static and a second one throws `Host exists already`.

## CI

`.github/workflows/ci.yml` runs on pushes to `master` and on PRs, with five jobs:

1. `version` — bumps the patch component in `Directory.Build.props` on a master push unless that commit edited the file by hand, then commits the bump as `[skip ci]` so it starts no run of its own. Only this job has `contents: write`.
2. `setup` — builds the solution, downloads the Godot mono build, imports the three Godot projects.
3. `discover` — builds the test matrix from the adapter's own `Discover: TestSuite ... with N TestCases found.` lines, so the suite list can never drift from the code.
4. `test` — one job per suite, each running `dotnet test --filter FullyQualifiedName~<Suite>` under `xvfb-run`, then asserting it reported as many tests as were discovered. Splitting per suite is mandatory, see "Running suites".
5. `pack` — master pushes only: `dotnet pack` the three libraries and push to nuget.org with `secrets.NUGET_API_KEY` and `--skip-duplicate`.

Add the `NUGET_API_KEY` repository secret before the first master push, otherwise the publish step fails loudly on purpose. If `Godot.NET.Sdk` changes in a csproj, update `GODOT_URL` in the workflow — nothing enforces the coupling.

## Docs (DocFX)

- Config: `docs/docfx.json`; CI (`.github/workflows/docs.yml`) runs `docfx docs/docfx.json` with docfx pinned to `2.81.0` and deploys `docs/_site` to GitHub Pages on push to `master`.
- Verify docs changes locally with `dotnet tool install --global docfx --version 2.81.0` then `docfx docs/docfx.json` (a successful run reports 0 warnings/0 errors). If the tool's apphost can't find .NET (Homebrew/arm64 installs), run its dll instead: `dotnet <tool-path>/.store/docfx/2.81.0/docfx/2.81.0/tools/net10.0/any/docfx.dll docs/docfx.json`.
- `docs/api/` and `docs/_site/` are **generated and gitignored** — never edit or commit them. `docs/toc.yml` links `api/toc.yml`, which only exists after a docfx run.
- Adding a guide = new `docs/**/*.md` **plus** an entry in `docs/toc.yml`, otherwise it is unreachable from the site.
- Keep `src/<Pkg>/src/README.md` and root `README.md` in sync with `docs/<Pkg>/index.md` / `docs/index.md` — they ship in the packages and the repo front page.
- Public API needs XML doc comments (`GenerateDocumentationFile`); DocFX renders them as the API reference. Docs drifted from the API before (renamed attributes, changed builder methods), so verify every API name against `src/` before writing it down.

## Conventions

- `.editorconfig`: 4 spaces for `.cs`, braces on the same line, explicit types (`var` is disabled), private fields `_camelCase`, static readonly ALL_CAPS. `.csproj`/`.json` use 2 spaces.
- The `.editorconfig` `end_of_line = crlf` + `insert_final_newline = false` settings do **not** match the repo (files are LF with a trailing newline). Don't run `dotnet format` or reformat files wholesale.
- `NoWarn` includes `8618` (uninitialized non-nullable fields) — intentional, several fields are assigned by the engine/host.
- Godot version is pinned twice: `GodotSharp 4.7.2` / `Godot.NET.Sdk/4.7.2` in the csproj files. The test `project.godot` files still advertise `config/features=PackedStringArray("4.2", ...)` — stale, harmless.

## Behavioural gotchas

- `Godex.Hosting.Host` is effectively a singleton: `Host.ServiceProvider` is static and `_EnterTree()` throws `InvalidOperationException` if a second `Host` enters the tree. Docs must not claim multiple coexisting hosts.
- `Resolve()` (`NodeExtensions`) resolves node paths, bit flags and groups in one shot, is idempotent (tracked via node metadata `resolved`), and is called automatically for nodes created by `GDx.New(...)`.