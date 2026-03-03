using FrienDex.Data;
using FrienDex.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FrienDex.Services
{
    public class PersonRepo : IPersonRepo
    {
        private readonly DexContext _db;

        public PersonRepo(DexContext db)
        {
            _db = db;
        }

        public async Task<Person> CreateAsync(Person newPerson)
        {
            await _db.People.AddAsync(newPerson);
            await _db.SaveChangesAsync();
            return newPerson;
        }

        public async Task<ICollection<Person>> ReadAllAsync()
        {
            return await _db.People.ToListAsync();
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
