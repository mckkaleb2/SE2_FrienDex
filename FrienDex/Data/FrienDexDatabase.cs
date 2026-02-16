using FrienDex.Models.Entities;
using Microsoft.Extensions.Logging;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
//using static Java.Util.Jar.Attributes;

namespace FrienDex.Data
{
    public class FrienDexDatabase
    {
        SQLiteAsyncConnection database;

        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrienDexDatabase"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public FrienDexDatabase(ILogger<FrienDexDatabase> logger)
        {
            _logger = logger;
        }


        private async Task Init()
        {
            if (database is not null)
                return;

            try
            {
                // Establish connection Asyncronously
                database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
                // Create Tables
                var result = await database.CreateTableAsync<TestItem>();
            } //end try
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating tables");
                throw;
            }
        }

        public string StatusMessage { get; set; }

#region DataManipulation
        // ORM for storing and retrieving objects without SQL statements

        public async Task<List<TestItem>> GetItemsAsync()
        {
            try
            {
                await Init();
                return await database.Table<TestItem>().ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
                return new List<TestItem>();
            }
        }

        // TODO : Add a method to get Items using a property. See: Getting all PhoneNumbers by PersonId


        public async Task<List<TestItem>> GetItemsNotDoneAsync()
        {
            try
            {
                await Init();
                return await database.Table<TestItem>().Where(t => t.Done).ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data not done. {0}", ex.Message);
                return new List<TestItem>();

            }


            // SQL queries are also possible
            //return await database.QueryAsync<TodoItem>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        }

        public async Task<TestItem?> GetItemAsync(int id)
        {
            try
            {
                await Init();
                return await database.Table<TestItem>().Where(i => i.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data for id: \"{0}\". {1}", id, ex.Message);
                return null;
            }

        }

        public async Task<int> SaveItemAsync(TestItem item)
        {
            try
            {
                await Init();
                if (item.Id != 0)
                {
                    return await database.UpdateAsync(item);
                }
                else
                {
                    return await database.InsertAsync(item);
                }
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to save data for id: \"{0}\". {1}", item.Id, ex.Message);
                return -1;
            }
        }

        public async Task<int> DeleteItemAsync(TestItem item)
        {
            try
            {
                await Init();
                return await database.DeleteAsync(item);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to delete data for id: \"{0}\". {1}", item.Id, ex.Message);
                return -1;

            }
        }
#endregion DataManipulation

    } //end class
}

