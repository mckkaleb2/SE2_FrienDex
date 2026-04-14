using FrienDex.Data;
using FrienDex.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FrienDex.Services
{
    public partial class PersonRepo : IPersonRepo
    {
        private readonly DexContext _db;
        private readonly ILogger<Person> _logger;
        private readonly SemaphoreSlim _dbLock = new(1, 1);

        public PersonRepo(DexContext db, ILogger<Person> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<Person> CreateAsync(Person newPerson)
        {
            await _dbLock.WaitAsync();
            try
            {
                await _db.People.AddAsync(newPerson);
                await _db.SaveChangesAsync();

                var tempPerson = await _db.People
                    .FirstOrDefaultAsync(p => p.Id == newPerson.Id);

                await _db.Set<DexEntry>().AddAsync(new DexEntry { Person = tempPerson });
                await _db.SaveChangesAsync();

#if DEBUG
                System.Diagnostics.Debug.WriteLine("\n\n\n\t\tATTEMPTING TO CREATE A PERSON\n\n\n");
#endif
                return newPerson;
            }
            finally
            {
                _dbLock.Release();
            }
        }

        public async Task<IEnumerable<Person>> CreateWithListAsync(List<Person> newPeople)
        {
            var returnedPeople = new List<Person>();

            await _dbLock.WaitAsync();
            try
            {
                foreach (var person in newPeople)
                {
                    await _db.People.AddAsync(person);
                    returnedPeople.Add(person);

                    try
                    {
                        await _db.SaveChangesAsync();
                        System.Diagnostics.Debug.WriteLine("Person created & saved in the Database using batch method.");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(
                            $"\n\n---------------- CreateUserDexHolder (batch) --- log ---------\n\n" +
                            $"An error occurred while adding lists to the database. And saving a person to the DB {ex.Message}" +
                            "\n\n\t{FirstName} {LastName}\n\n" +
                            $"\n\n----- 1 -------- CreateUserDexHolder (batch) --- console ----\n\n" +
                            $"{ex}" +
                            $"\n\n---- end ------- CreateUserDexHolder (batch) --- console ----\n\n",
                            person.FirstName, person.LastName);
                    }
                }
            }
            finally
            {
                _dbLock.Release();
            }

            return returnedPeople;
        }

        //public async Task<List<Person>> ReadAllAsync()
        //{
        //    await _dbLock.WaitAsync();
        //    try
        //    {
        //        return await _db.People
        //            .AsNoTracking()
        //            .ToListAsync();
        //    }
        //    finally
        //    {
        //        _dbLock.Release();
        //    }
        //}

        public async Task<ICollection<Person>> ReadAllHierarchyAsync()
        {
            await _dbLock.WaitAsync();
            try
            {
                return await _db.People
                    .Include(p => p.DexEntry)
                    .ThenInclude(e => e.Blocks)
                    .Include(p => p.Rooms)
                    .ToListAsync();
            }
            finally
            {
                _dbLock.Release();
            }
        }

        public async Task<Person?> ReadAsync(int id)
        {
            System.Diagnostics.Debug.WriteLine($"[PersonRepo] ReadAsync START: {DateTime.Now:HH:mm:ss.fff}");
            var person = await _db.People
                .Include(p => p.DexEntry)
                .ThenInclude(e => e.Blocks)
                .FirstOrDefaultAsync(p => p.Id == id);
            System.Diagnostics.Debug.WriteLine($"[PersonRepo] ReadAsync END: {DateTime.Now:HH:mm:ss.fff}");
            return person;
        }

        public async Task<List<Person>> ReadAllAsync()
        {
            System.Diagnostics.Debug.WriteLine($"[PersonRepo] ReadAllAsync START: {DateTime.Now:HH:mm:ss.fff}");
            var result = await _db.People.AsNoTracking().ToListAsync();
            System.Diagnostics.Debug.WriteLine($"[PersonRepo] ReadAllAsync END: {DateTime.Now:HH:mm:ss.fff}");
            return result;
        }


        public async Task UpdateAsync(int id, Person person)
        {
            await _dbLock.WaitAsync();
            try
            {
                var existingPerson = await _db.People.FindAsync(id);
                if (existingPerson != null)
                {
                    _db.Entry(existingPerson).CurrentValues.SetValues(person);
                    await _db.SaveChangesAsync();
                }
            }
            finally
            {
                _dbLock.Release();
            }
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
