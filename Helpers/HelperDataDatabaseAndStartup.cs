using System.Data.SQLite;

// ReSharper disable InconsistentNaming

namespace HLWebScraper.Net.Helpers;

internal static class HelperDataDatabaseAndStartup
{
    /// <summary>
    ///     Creates the SQLite DB if it doesn't exist
    /// </summary>
    internal static void DataCreateSQLiteDB()
    {
        try
        {
            // create folder in Appdata if doesn't exist
            FileInfo userDataBaseFile = new(fileName: HelperVariables.SettingsDatabaseFilePath);

            if (userDataBaseFile is { Exists: true, Length: 0 }) userDataBaseFile.Delete();

            if (!userDataBaseFile.Exists)
            {
                try
                {
                    SQLiteConnection.CreateFile(
                        databaseFileName: Path.Combine(paths: HelperVariables.SettingsDatabaseFilePath));
                    SQLiteConnection sqliteDB = new(connectionString: @"Data Source=" +
                                                                      Path.Combine(paths: HelperVariables
                                                                         .SettingsDatabaseFilePath) + "; Version=3");
                    sqliteDB.Open();

                    string sql = """
                                 CREATE TABLE settings(
                                     settingId TEXT(255)         NOT NULL,
                                     settingValue NTEXT(2000)    DEFAULT "",
                                     PRIMARY KEY(settingId)
                                     )
                                 ;
                                 """;
                    SQLiteCommand sqlCommandStr = new(commandText: sql, connection: sqliteDB);
                    sqlCommandStr.ExecuteNonQuery();
                    sqliteDB.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(text: ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(text: ex.Message);
        }
    }

    /// <summary>
    ///     Fills the SQLite database with defaults
    /// </summary>
    internal static void DataWriteSQLiteSettingsDefaultSettings()
    {
        Dictionary<string, string> defaultSettingsDictionary = new()
        {
            { "Theme", "Light" },
            { "MaxConnectionsPerServer", "50" }
        };

        foreach (KeyValuePair<string, string> keyValuePair in defaultSettingsDictionary)
            if (string.IsNullOrWhiteSpace(
                    value: HelperDataApplicationSettings.DataReadSQLiteSettings(tableName: "settings",
                        settingId: keyValuePair.Key, returnBlankIfNull: true)))
                UpdateSQLite(settingId: keyValuePair.Key, controlDefaultValue: keyValuePair.Value);


        void UpdateSQLite(string settingId,
            string controlDefaultValue)
        {
            HelperDataApplicationSettings.DataWriteSQLiteSettings(tableName: "settings",
                settingId: settingId,
                settingValue: controlDefaultValue);
        }
    }
}