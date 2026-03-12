//using Android.Content;
using FrienDex.Data;
using FrienDex.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FrienDex.Services
{
    public partial class PersonRepo : IPersonRepo
    {
        private readonly DexContext _db;
        private readonly ILogger<Person> _logger;


        public PersonRepo(
            DexContext db,
            ILogger<Person> logger
            )
        {
            _db = db;
            _logger = logger;
        }

        public async Task<Person> CreateAsync(Person newPerson)
        {
            await _db.People.AddAsync(newPerson);
            await _db.SaveChangesAsync();
            var tempPerson = _db.People.FirstOrDefault(p => p.Id == newPerson.Id);
            await _db.Set<DexEntry>().AddAsync(new DexEntry { Person = tempPerson });
            await _db.SaveChangesAsync();

#if DEBUG
            System.Diagnostics.Debug.WriteLine($"\n\n\n\t\tATTEMPTING TO CREATE A PERSON\n\n\n");
#endif
            return newPerson;
        }



        public async Task<IEnumerable<Person>> CreateWithListAsync(List<Person> newPeople)
        {
            // Create a list to people to return
            var returnedPeople = new List<Person>();

            // in case we have to modify to add multiple entities at once
            // Zip allows iteration of multiple arrays at once. If arrays are different lengths, it will evaluate to the length of the shortest array
            // source: https://stackoverflow.com/a/41534351
            // NOTE: was applicable when creating a multi-user app (thx AspNetCore.Identity :/ )
            //          but may not be applicable here. Feel free to remove this comment if you think it's unnecessary

            foreach (var person in newPeople)
            {
                await _db.People.AddAsync(person);
                returnedPeople.Add(person);
                try
                {
                    await _db.SaveChangesAsync();
                    //_logger.LogInformation("Person created & saved in the Database using batch method.");
                    System.Diagnostics.Debug.WriteLine("Person created & saved in the Database using batch method.");

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(//_logger.LogError(
                        $"\n\n---------------- CreateUserDexHolder (batch) --- log ---------\n\n"
                        + $"An error occurred while adding lists to the database. And saving a person to the DB {ex.Message}"
                        //        +$"\n\n---------------- SEED DATA ASYNC --- log ---------\n\n");
                        
                        + "\n\n\t{FirstName} {LastName}\n\n"
                        //    System.Diagnostics.Debug.WriteLine(
                        + $"\n\n----- 1 -------- CreateUserDexHolder (batch) --- console ----\n\n"
                        //        + $"An error occurred while adding lists to the database. {ex.Message}"
                        //        + $"\n\n----- 2 -------- SEED DATA ASYNC --- console ----\n\n"
                        + $"{ex}"
                        + $"\n\n---- end ------- CreateUserDexHolder (batch) --- console ----\n\n"
                        , person.FirstName, person.LastName);

                }
            }

            return returnedPeople;
        }

        public async Task<List<Person>> ReadAllAsync()
        {
            return await _db.People.ToListAsync();
        }

        public async Task<ICollection<Person>> ReadAllHierarchyAsync()
        {
            return await _db.People
                .Include(p => p.DexEntry)
                .ThenInclude(e => e.Blocks)
                .Include(p => p.Rooms)
                .ToListAsync();
        }


        public Task<Person?> ReadAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(int id, Person person)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
