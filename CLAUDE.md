# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

Unofficial WinForms (.NET 8, `net8.0-windows10.0.17763.0`, win-x64) app that scrapes securities (not funds) from the Hargreaves Lansdown website, classifies them (ETF types via user CSV), and exports to CSV. Ported from VBA; deliberately simple code style. No tests or linter exist in the repo.

## Commands

- Build: `dotnet build HLWebScraper.Net.csproj` (Debug) / `-c Release`
- Run: `dotnet run --project HLWebScraper.Net.csproj` (Windows only; GUI app)
- Every build also publishes a single-file exe (framework-dependent) to `<OutDir>\single\` via the `SingleFileAfterBuild` target in the csproj.
- `Directory.Build.props` redirects `obj\` and `bin\` to `%LOCALAPPDATA%\build\HLWebScraper.Net\` (repo lives on Google Drive, which locks obj folders). Look there for build artifacts, not in the repo.

## Architecture

- `Program.cs` → `FrmMainApp` (almost all logic lives in this one form; `.Designer.cs` is generated, avoid hand-editing).
- Scrape pipeline in `FrmMainApp.cs`: `CollateHLStocksByLetterAsync` walks HL's A–Z search listings in batches (deliberately throttled to avoid server rate limiting; `MaxConnectionsPerServer` setting, default 50) → `FireAndForgetAsync`/`GetHtmlAsync` download pages → HTML is stored in `CompressedMemoryCache` (`Helpers/`, Apache-2.0 third-party code; compression is required because tens of thousands of 200–400KB pages would exhaust RAM) → parsing → `WriteOutputToCSV`. FX rates come from `ReadJsonFXFromWebAsync` for GBP conversion.
- `Helpers/HLPageParser.cs` (`HlPageParser`) extracts data from the `__NEXT_DATA__` JSON script embedded in each HL page (`pageProps.investmentDetails`, `keyMetrics`, `instrumentInfoOverview`), with fallbacks across several JSON paths. When HL changes its site, this is the first place to fix.
- `Model/TagsToModelValueTransformations.cs` and `Helpers/HelperStringUtils.cs` convert the parsed dictionaries into typed model values and apply validity rules (securities with invalid Ticker/Name/SEDOL or no KID are discarded). Ticker is taken from the last parentheses in the page header.
- ETF classification: `Resources/ETF_Types.csv` (user-made) overrides `ETF_Types_example.csv`; case-insensitive "contains" match, first match in file order wins.
- Settings persist in SQLite at `%APPDATA%\HLWebScraper.Net\database.sqlite` (`HelperData*` classes, `HelperVariables`).
- Dark mode uses `WinFormsDarkThemerNinja` plus a custom menu renderer in `FrmMainApp`.

## Conventions

- Named arguments are used throughout C# calls (e.g. `Path.Combine(path1: ..., path2: ...)`); match this style.
- Output CSV consumers are told to rely on column names, not order.
- Cancellation (Stop button) only covers scraping, not parsing.
- Release notes go in `changelog.md` (build-number versioned, starting at v4); `readme.md` and `changelog.md` are copied to the output dir.
