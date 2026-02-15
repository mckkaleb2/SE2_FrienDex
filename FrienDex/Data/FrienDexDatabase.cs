using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using FrienDex.Models;

namespace FrienDex.Data
{
    public class FrienDexDatabase
    {
        SQLiteAsyncConnection database;

        async Task Init()
        {
            if (database is not null)
                return;

            database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            
            // Create Tables
            var result = await database.CreateTableAsync<TestItem>();
        }
        
        
        #region DataManipulation
        // ORM for storing and retrieving objects without SQL statements

        public async Task<List<TestItem>> GetItemsAsync()
        {
            await Init();
            return await database.Table<TestItem>().ToListAsync();
        }

        public async Task<List<TestItem>> GetItemsNotDoneAsync()
        {
            await Init();
            return await database.Table<TestItem>().Where(t => t.Done).ToListAsync();

            // SQL queries are also possible
            //return await database.QueryAsync<TodoItem>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        }

        public async Task<TestItem> GetItemAsync(int id)
        {
            await Init();
            return await database.Table<TestItem>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(TestItem item)
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

        public async Task<int> DeleteItemAsync(TestItem item)
        {
            await Init();
            return await database.DeleteAsync(item);
        }

        #endregion DataManipulation

    }
}

