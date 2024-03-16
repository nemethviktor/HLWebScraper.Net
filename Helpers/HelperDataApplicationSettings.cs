using System.Data.SQLite;

// ReSharper disable InconsistentNaming

namespace HLWebScraper.Net.Helpers;

internal static class HelperDataApplicationSettings
{
    /// <summary>
    ///     Reads the user-settings and returns them to the app (such as say default starting folder.)
    /// </summary>
    /// <param name="tableName">
    ///     This will generally be "settings" - imported from one of my other projects
    /// </param>
    /// <param name="settingId">Name of the SettingID for which data is requested</param>
    /// <param name="returnBlankIfNull"></param>
    /// <returns>String - the value of the given SettingID</returns>
    internal static string DataReadSQLiteSettings(string tableName,
        string settingId,
        bool returnBlankIfNull = false)
    {
        string returnString = string.Empty;

        using SQLiteConnection sqliteDB = new(connectionString: "Data Source=" + HelperVariables.SettingsDatabaseFilePath);
        sqliteDB.Open();

        string sqlCommandStr = @"
                               SELECT settingValue
                               FROM " + tableName +
                               Environment.NewLine +
                               @"WHERE 1=1
                                    AND settingId = @settingId 
                                    ;"
            ;
        SQLiteCommand sqlToRun = new(commandText: sqlCommandStr, connection: sqliteDB);
        sqlToRun.Parameters.AddWithValue(parameterName: "@settingId", value: settingId);

        using SQLiteDataReader reader = sqlToRun.ExecuteReader();
        while (reader.Read())
        {
            returnString = reader.GetString(i: 0);
        }

        if (returnBlankIfNull && string.IsNullOrWhiteSpace(value: returnString)) returnString = string.Empty;

        return returnString;
    }


    /// <summary>
    ///     Similar to the one above (which reads the data) - this one writes it.
    /// </summary>
    /// <param name="tableName">
    ///     This will generally be "settings" (but could be applayout as well). Remainder of an older
    ///     design where I had tables for data lined up to be saved
    /// </param>
    /// <param name="settingId">Name of the SettingID for which data is requested</param>
    /// <param name="settingValue">The value to be stored.</param>
    internal static void DataWriteSQLiteSettings(string tableName,
        string settingId,
        string settingValue)
    {
        DataDeleteSQLiteSettings(tableName: tableName, settingId: settingId); // triple-check dev lameness

        using SQLiteConnection sqliteDB =
            new(connectionString: "Data Source=" +
                                  HelperVariables.SettingsDatabaseFilePath);
        sqliteDB.Open();

        string sqlCommandStrCMD = @"
                                INSERT INTO " +
                                  tableName + " " +
                                  " ( settingId, settingValue) " +
                                  "VALUES (@settingId, @settingValue);"
            ;

        SQLiteCommand sqlCommandStr = new(commandText: sqlCommandStrCMD, connection: sqliteDB);
        sqlCommandStr.Parameters.AddWithValue(parameterName: "@settingId", value: settingId);
        sqlCommandStr.Parameters.AddWithValue(parameterName: "@settingValue", value: settingValue);
        sqlCommandStr.ExecuteNonQuery();
    }

    private static void DataDeleteSQLiteSettings(string tableName,
        string settingId)
    {   
        using SQLiteConnection sqliteDB = new(connectionString: "Data Source=" + HelperVariables.SettingsDatabaseFilePath);
        sqliteDB.Open();

        string sqlCommandStrCMD = @"
                                DELETE FROM " +
                                  tableName + " " +
                                  "WHERE settingId = @settingId;"
            ;

        SQLiteCommand sqlCommandStr = new(commandText: sqlCommandStrCMD, connection: sqliteDB);
        sqlCommandStr.Parameters.AddWithValue(parameterName: "@settingId", value: settingId);
        sqlCommandStr.ExecuteNonQuery();
    }

    /// <summary>
    ///     This is largely me being a derp and doing a manual cleanup. My original SQL script was a bit buggy and so we have a
    ///     potential plethora of unused and possibly errouneous setting tokens.
    /// </summary>
    internal static void DataDeleteSQLitesettingsCleanup()
    {
        using SQLiteConnection sqliteDB =
            new(connectionString: "Data Source=" +
                                  HelperVariables.SettingsDatabaseFilePath);
        sqliteDB.Open();

        string sqlCommandStr = @"
                                DELETE 
                                FROM   [settings]
                                WHERE  [rowid] NOT IN (SELECT MAX ([rowid])
                                       FROM   [settings]
                                       GROUP  BY [settingId]);
                                "
            ;
        sqlCommandStr += ";";
        SQLiteCommand sqlToRun = new(commandText: sqlCommandStr, connection: sqliteDB);
        sqlToRun.ExecuteNonQuery();
    }

    /// <summary>
    ///     This just compresses the database. Though I don't expect it'd be a large file in the first place but unlikely to
    ///     hurt.
    /// </summary>
    internal static void DataVacuumDatabase()
    {
        using SQLiteConnection sqliteDB =
            new(connectionString: "Data Source=" +
                                  HelperVariables.SettingsDatabaseFilePath);
        sqliteDB.Open();

        string sqlCommandStr = @"VACUUM;"
            ;
        sqlCommandStr += ";";
        SQLiteCommand sqlToRun = new(commandText: sqlCommandStr, connection: sqliteDB);
        sqlToRun.ExecuteNonQuery();
    }
}