using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Microsoft.Extensions.Logging;

using SQLite;
using FrienDex.Models.Entities;


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
        private SQLiteAsyncConnection connAsync;

        private async Task Init()
        {
            /*if (_hasBeenInitialized)
                return;*/
            if (connAsync != null)
            {
                await Console.Out.WriteLineAsync($"DB Path: {_dbPath} Exists:{File.Exists(_dbPath)} Size:{(File.Exists(_dbPath) ? new FileInfo(_dbPath).Length : 0)}");
                return;
            }
                //conn = new SQLiteAsyncConnection(Constants.DatabasePath);
                //await connection.OpenAsync();

                try
                {
                    connAsync = new SQLiteAsyncConnection(_dbPath);
                    await connAsync.CreateTableAsync<TestItem>();

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

                    await Console.Out.WriteLineAsync($"DB Path: {_dbPath} Exists:{File.Exists(_dbPath)} Size:{(File.Exists(_dbPath) ? new FileInfo(_dbPath).Length : 0)}");

                } //end try
                catch (Exception e)
                {
                    _logger.LogError(e, "Error creating tables");
                    throw;
                }

            _hasBeenInitialized = true;
        }


        private void InitSync()
        {
            if (conn != null)
                return;
            try
            {
                conn = new SQLiteConnection(_dbPath);
                conn.CreateTable<TestItem>();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating tables");
                throw;
            }
        }




        public TestItemRepository(string dbPath)
        {
            _dbPath = dbPath;
        }

        /// <summary>
        /// A method to add a new TestItem to the database. (NOTE: Use Async version of method unless absolutely necessary) It takes a name as a parameter, performs basic validation, and then inserts the new TestItem into the database. The result of the operation is stored in the StatusMessage property for feedback.
        /// </summary>
        /// <param name="name"></param>
        public void AddNewTestItem(string name)
        {
            int result = 0;
            try
            {
                // Call Init()
                InitSync();

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

        /// <summary>
        /// Adds a new test item to the database asynchronously using the specified name.
        /// </summary>
        /// <remarks>If the operation succeeds, the status message is updated to indicate the number of
        /// records added. If the operation fails, the status message contains the error details.</remarks>
        /// <param name="name">The name of the test item to add. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task AddNewTestItemAsync(string name)
        {
            int result = 0;
            try
            {
                // Call Init()
                await Init();

                // basic validation to ensure a name was entered
                if (string.IsNullOrEmpty(name))
                    throw new Exception("Valid name required");

                // Insert the new person into the database
                result = await connAsync.InsertAsync(new TestItem { Name = name });
                StatusMessage = string.Format("{0} record(s) added (Name: {1})", result, name);

            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", name, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all test items from the database. (NOTE: Use Async version of method unless absolutely necessary) 
        /// </summary>
        /// <remarks>If an error occurs while accessing the database, the method returns an empty list and
        /// updates the <c>StatusMessage</c> property with the error details. The returned list will never be <see
        /// langword="null"/>.</remarks>
        /// <returns>A list of <see cref="TestItem"/> objects representing all test items in the database. Returns an empty list
        /// if no items are found or if an error occurs during retrieval.</returns>
        public List<TestItem> GetAllTestItems()
        {
            // Init then retrieve a list of TestItem objects from the database into a list
            try
            {
                InitSync();
                return conn.Table<TestItem>().ToList();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }

            return new List<TestItem>();
        }

        /// <summary>
        /// Asynchronously retrieves all test items from the database.
        /// </summary>
        /// <remarks>If an error occurs while accessing the database, the method returns an empty list and
        /// updates the <c>StatusMessage</c> property with the error details. The returned list will never be <see
        /// langword="null"/>.</remarks>
        /// <returns>A list of <see cref="TestItem"/> objects representing all test items in the database. Returns an empty list
        /// if no items are found or if an error occurs during retrieval.</returns>
        public async Task<List<TestItem>> GetAllTestItemsAsync()
        {
            // Init then retrieve a list of TestItem objects from the database into a list
            try
            {
                await Init();
                return await connAsync.Table<TestItem>().ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }

            return new List<TestItem>();
        }


    }
}
