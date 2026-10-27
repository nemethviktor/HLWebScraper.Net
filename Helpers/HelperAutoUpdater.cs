using AutoUpdaterDotNET;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace HLWebScraper.Net.Helpers;

/// <summary>
///     Self-update via AutoUpdater.NET, fed straight from the GitHub "latest release" API.
///     GitHub releases are tagged "b[build]" (e.g. b9794) and carry a .zip asset with the single-file exe.
///     The build number is days since 2000-01-01 (see AssemblyVersion "4.0.*"), so the tag is all we need to compare.
/// </summary>
internal static class HelperAutoUpdater
{
    private const string Repo = "nemethviktor/HLWebScraper.Net";
    private const string LastCheckSettingId = "LastUpdateCheck";
    private static readonly TimeSpan CheckInterval = TimeSpan.FromDays(value: 7);
    private const string LatestReleaseUrl = $"https://api.github.com/repos/{Repo}/releases/latest";

    /// <summary>
    ///     Kicks off an (asynchronous) update check, at most once a week. Never throws: a failed check just means no update prompt.
    /// </summary>
    internal static void LaunchAutoUpdater()
    {
        try
        {
#if !DEBUG
            if (long.TryParse(s: HelperDataApplicationSettings.DataReadSQLiteSettings(tableName: "settings",
                                  settingId: LastCheckSettingId, returnBlankIfNull: true),
                    result: out long lastCheck) &&
                DateTimeOffset.UtcNow - DateTimeOffset.FromUnixTimeSeconds(seconds: lastCheck) < CheckInterval)
            {
                return;
            }
#endif
#if DEBUG
            // AutoUpdater.InstalledVersion = new Version(version: "4.0.9000.0"); // here for testing only.
#endif
            AutoUpdater.ParseUpdateInfoEvent += OnParseUpdateInfo;
            // GitHub's API rejects requests without a User-Agent
            AutoUpdater.HttpUserAgent = "HLWebScraper.Net";
            AutoUpdater.ReportErrors = false;
            AutoUpdater.Start(appCast: LatestReleaseUrl);

            HelperDataApplicationSettings.DataWriteSQLiteSettings(tableName: "settings",
                settingId: LastCheckSettingId,
                settingValue: DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());
        }
        catch (Exception)
        {
            // updating is a nicety, never block startup on it
        }
    }

    /// <summary>
    ///     Translates GitHub's release JSON into the structure AutoUpdater.NET expects.
    /// </summary>
    private static void OnParseUpdateInfo(ParseUpdateInfoEventArgs args)
    {
        JObject release = JObject.Parse(json: args.RemoteData);

        string tag = release.Value<string>(key: "tag_name") ?? "";
        if (release.Value<bool>(key: "prerelease") ||
            !int.TryParse(s: Regex.Replace(input: tag, pattern: "[^0-9]", replacement: ""), result: out int remoteBuild))
        {
            return; // leave UpdateInfo null: nothing to offer
        }

        string? downloadUrl = release["assets"]?
                              .Select(selector: asset => asset.Value<string>(key: "browser_download_url"))
                              .FirstOrDefault(predicate: url => url != null &&
                                                                url.EndsWith(value: ".zip",
                                                                    comparisonType: StringComparison.OrdinalIgnoreCase));
        if (downloadUrl == null)
        {
            return;
        }

        Version local = Assembly.GetEntryAssembly()!.GetName().Version!;

        // Remote major/minor can't be queried, so borrow the local ones. Revision 0 means that an installed build
        // with the same build number (revision > 0) is never seen as outdated.
        args.UpdateInfo = new UpdateInfoEventArgs
        {
            CurrentVersion = $"{local.Major}.{local.Minor}.{remoteBuild}.0",
            DownloadURL = downloadUrl,
            ChangelogURL = $"https://github.com/{Repo}/blob/{tag}/changelog.md"
        };
    }
}
