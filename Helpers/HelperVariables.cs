using static System.Environment;

namespace HLWebScraper.Net.Helpers;

internal static class HelperVariables
{
    internal static bool UserSettingUseDarkMode;

    private static readonly string UserDataFolderPath = GetRoamingFolderString();

    internal static readonly string SettingsDatabaseFilePath =
        GetSettingsDatabaseFilePath();


    /// <summary>
    ///     Gets the app's resources folder location.
    /// </summary>
    /// <returns>The app's resources folder location.</returns>
    internal static string GetResourcesFolderString()
    {
        return
            Path.Combine(path1: AppDomain.CurrentDomain.BaseDirectory,
                         path2: "Resources");
    }

    /// <summary>
    ///     Pulls (and creates if necessary) the Roaming/Users subfolder for the app.
    /// </summary>
    /// <returns>Path name of the Roaming/Users subfolder</returns>
    private static string GetRoamingFolderString()
    {
        string userDataFolderPath = Path.Combine(
            path1: GetFolderPath(folder: SpecialFolder.ApplicationData),
            path2: "HLWebScraper.Net");

        if (!Directory.Exists(path: userDataFolderPath)) Directory.CreateDirectory(path: userDataFolderPath);

        return userDataFolderPath;
    }


    /// <summary>
    ///     Gets the sqlite file location containing the database info.
    /// </summary>
    /// <returns>The sqlite file location containing the database info</returns>
    private static string GetSettingsDatabaseFilePath()
    {
        return Path.Combine(
            path1: UserDataFolderPath, path2: "database.sqlite");
    }
}