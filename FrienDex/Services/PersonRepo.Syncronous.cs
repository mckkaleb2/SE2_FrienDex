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
        public Person CreateSyncronous(Person newPerson)
        {
            _db.People.Add(newPerson);
            _db.SaveChanges();
            return newPerson;
        }

        public IEnumerable<Person> CreateWithListSyncronous(List<Person> newPeople)
        {
            var returnedPeople = new List<Person>();
            foreach (var person in newPeople)
            {
                _db.People.Add(person);
                returnedPeople.Add(person);
                try
                {
                    _db.SaveChanges();
                    _logger.LogInformation("Person created & saved in the Database using batch method.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        $"\n\n---------------- CreatePerson (batch) --- log ---------\n\n"
                        + $"An error occurred while adding lists to the database. And saving a person to the DB {ex.Message}"
                        //        +$"\n\n---------------- SEED DATA ASYNC --- log ---------\n\n");
                        + "\n\n\t{FirstName} {LastName}\n\n"
                        //    Console.WriteLine(
                        + $"\n\n----- 1 -------- CreatePerson (batch) --- console ----\n\n"
                        //        + $"An error occurred while adding lists to the database. {ex.Message}"
                        //        + $"\n\n----- 2 -------- SEED DATA ASYNC --- console ----\n\n"
                        + $"{ex}"
                        + $"\n\n---- end ------- CreatePerson (batch) --- console ----\n\n"
                        , person.FirstName, person.LastName);
                }
            }
            return returnedPeople;
        }

        public Person? ReadSyncronous(int id)
        {
            return _db.People.Find(id);
        }

        public ICollection<Person> ReadAllSyncronous()
        {
            return _db.People.ToList();
        }



    }
}
