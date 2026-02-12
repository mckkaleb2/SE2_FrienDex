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
            var result = await database.CreateTableAsync<TestItemModel>();
        }
        
        
        #region DataManipulation


        public async Task<List<TestItemModel>> GetItemsAsync()
        {
            await Init();
            return await database.Table<TestItemModel>().ToListAsync();
        }

        public async Task<List<TestItemModel>> GetItemsNotDoneAsync()
        {
            await Init();
            return await database.Table<TestItemModel>().Where(t => t.Done).ToListAsync();

            // SQL queries are also possible
            //return await database.QueryAsync<TodoItem>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        }

        public async Task<TestItemModel> GetItemAsync(int id)
        {
            await Init();
            return await database.Table<TestItemModel>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(TestItemModel item)
        {
            await Init();
            if (item.ID != 0)
            {
                return await database.UpdateAsync(item);
            }
            else
            {
                return await database.InsertAsync(item);
            }
        }

        public async Task<int> DeleteItemAsync(TestItemModel item)
        {
            await Init();
            return await database.DeleteAsync(item);
        }

        #endregion DataManipulation

    }
}

