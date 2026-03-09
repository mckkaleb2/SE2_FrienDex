using System;
using System.Collections.Generic;
using System.Text;
using FrienDex.Data.Entities;

namespace FrienDex.Services
{
    public partial interface IPersonRepo
    {
        // CREATE
        Task<Person> CreateAsync(Person newPerson);

        // CREATE with Lists
        Task<IEnumerable<Person>> CreateWithListAsync(List<Person> newPeople);

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
