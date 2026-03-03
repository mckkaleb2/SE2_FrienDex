using System;
using System.Collections.Generic;
using System.Text;
using FrienDex.Data.Entities;

namespace FrienDex.Services
{
    public interface IPersonRepo
    {
        // CREATE
        Task<Person> CreateAsync(Person newPerson);

        // READALL
        Task<ICollection<Person>> ReadAllAsync();

        // READ
        Task<Person?> ReadAsync(int id);

        // UPDATE
        Task UpdateAsync(int id,  Person person);

        // DELETE
        Task DeleteAsync(int id);
    }
}
