using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Microsoft.Extensions.Logging;

using SQLite;
using FrienDex.Models;


namespace FrienDex.Data
{
    /// <summary>
    /// Repository class for managing TestItems in the database
    /// </summary>
    public class TestItemRepository
    {
        string _dbPath;


        private bool _hasBeenInitialized = false;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestItemRepository"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public TestItemRepository(ILogger<TestItemRepository> logger)
        {
            _logger = logger;
        }




        public string StatusMessage { get; set; }

        // Add variable for the SQLite connection
        private SQLiteConnection conn;

        private void Init()
        {
            // Add code to initialize the repository
            // 
            /*if (_hasBeenInitialized)
                return;*/
            if (conn != null)
                return;


            /*
                        await using var connection = new SqliteConnection(Constants.DatabasePath);
                        await connection.OpenAsync();
            */


            try
            {
                conn = new SQLiteConnection(_dbPath);
                conn.CreateTable<TestItem>();

                /*
                                var createTableCmd = connection.CreateCommand();
                                createTableCmd.CommandText = @"
                            CREATE TABLE IF NOT EXISTS Tag (
                                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                Title TEXT NOT NULL,
                                Color TEXT NOT NULL
                            );";
                                await createTableCmd.ExecuteNonQueryAsync();

                                createTableCmd.CommandText = @"
                            CREATE TABLE IF NOT EXISTS ProjectsTags (
                                ProjectID INTEGER NOT NULL,
                                TagID INTEGER NOT NULL,
                                PRIMARY KEY(ProjectID, TagID)
                            );";
                                await createTableCmd.ExecuteNonQueryAsync();
                */


            } //end try
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating tables");
                throw;
            }

            _hasBeenInitialized = true;
        }







        public TestItemRepository(string dbPath)
        {
            _dbPath = dbPath;
        }

        public void AddNewTestItem(string name)
        {
            int result = 0;
            try
            {
                // Call Init()
                Init();

                // basic validation to ensure a name was entered
                if (string.IsNullOrEmpty(name))
                    throw new Exception("Valid name required");

                // Insert the new person into the database
                result = conn.Insert(new TestItem { Name = name });
                StatusMessage = string.Format("{0} record(s) added (Name: {1})", result, name);
                // Reset for the next operation
                result = 0;
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", name, ex.Message);
            }
        }


        public List<TestItem> GetAllTestItems()
        {
            // TODO: Init then retrieve a list of TestItem objects from the database into a list
            try
            {
                Init();
                return conn.Table<TestItem>().ToList();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }

            return new List<TestItem>();
        }
    } 
}
