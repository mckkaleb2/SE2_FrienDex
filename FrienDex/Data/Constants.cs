namespace FrienDex.Data
{
    /// <summary>
    /// A class to keep track of information that is to stay constant, such as database path and filename
    /// </summary>
    public static class Constants
    {
        public const string DatabaseFilename = "AppSQLite.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |

            // TODO:We should add Protection flags before pushing to a final version

            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;


        public static string DatabasePath =>
            $"Data Source={Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename)}";
    }
}