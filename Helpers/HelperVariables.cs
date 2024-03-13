namespace HLWebScraper.Net.Helpers;

internal static class HelperVariables
{
    internal static bool UserSettingUseDarkMode;
    internal static readonly string ResourcesFolderPath = GetResourcesFolderString();


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
}