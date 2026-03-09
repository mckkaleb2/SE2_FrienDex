using System;
using System.Collections.Generic;
using System.Text;
using FrienDex.Data.Entities;


namespace FrienDex.Services
{
    public partial interface IPersonRepo
    {
        Person CreateSyncronous(Person newPerson);
        IEnumerable<Person> CreateWithListSyncronous(List<Person> newPeople);
        Person? ReadSyncronous(int id);
        ICollection<Person> ReadAllSyncronous();


    }
}
