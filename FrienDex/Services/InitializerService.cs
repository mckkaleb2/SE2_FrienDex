using System;
using System.Collections.Generic;
using System.Text;
using FrienDex.Data;
using FrienDex.Services;

namespace FrienDex.Services
{
    public class InitializerService : IInitializerService
    {
        private readonly DexContext _db;
        private readonly IPersonRepo _personRepo;

        public InitializerService(
            DexContext db, 
            IPersonRepo personRepo)
        {
            _db = db;
            _personRepo = personRepo;
        }



        #region asyncMethods
        public async Task SeedDataAsync()
        {
            try
            {
                await SeedPersonDataAsync();
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed, such as logging the error
                Console.WriteLine($"\n\n\n\nAn error occurred while seeding data: {ex.Message}\n\n\n\n\n");
            }
        } // END SeedDataAsyncAsync METHOD

        #endregion asyncMethods
        // -------------------------
        #region syncronousMethods

        public async Task SeedPersonDataAsync() {
            try
            {
                var people = await _personRepo.ReadAllAsync();
                if (!people.Any())
                {
                    // could also use the _personRepo.CreateWithListsAsync() method here, but the autofill suggestion works for now

                    var person1 = new Data.Entities.Person
                    {
                        FirstName = "John",
                        LastName = "Doe",
                        IsFavorite = true
                    };
                    var person2 = new Data.Entities.Person
                    {
                        FirstName = "Jane",
                        LastName = "Smith",
                        IsFavorite = false
                    };
                    await _personRepo.CreateAsync(person1);
                    await _personRepo.CreateAsync(person2);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed, such as logging the error
                Console.WriteLine($"\n\n\n\nAn error occurred while seeding person data: {ex.Message}\n\n\n\n\n");
            }
            
        
        } // END SeedPersonDataAsync METHOD

        public void SeedDataSyncronous()
        {
            try
            {
                SeedPersonDataSyncronous();
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed, such as logging the error
                Console.WriteLine($"\n\n\n\nAn error occurred while seeding data: {ex.Message}\n\n\n\n\n");
            }
        } // END SeedDataSyncronous METHOD


        public void SeedPersonDataSyncronous()
        {
            try
            {
                var people = _personRepo.ReadAllSyncronous();
                if (!people.Any())
                {
                    var person1 = new Data.Entities.Person
                    {
                        FirstName = "John",
                        LastName = "Doe",
                        IsFavorite = true
                    };
                    var person2 = new Data.Entities.Person
                    {
                        FirstName = "Jane",
                        LastName = "Smith",
                        IsFavorite = false
                    };
                    _personRepo.CreateSyncronous(person1);
                    _personRepo.CreateSyncronous(person2);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed, such as logging the error
                Console.WriteLine($"\n\n\n\nAn error occurred while seeding person data: {ex.Message}\n\n\n\n\n");
            }
        } // END SeedPersonDataSyncronous METHOD

        #endregion syncronousMethods

    }// END InitializerService CLASS
}
