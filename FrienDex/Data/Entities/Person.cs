using System;
using System.Collections.Generic;
using System.Text;

namespace FrienDex.Data.Entities
{
    public class Person
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool? IsFavorite { get; set; }
        // These could be added to an Entry. Doesn't need to be associated with Person.
        // public DateTime? Birthday { get; set; }
        // public string? Gender { get; set; }
        // public string? Pronouns { get; set; }
        // public int? Ratings { get; set; }


        // public List<Room> Rooms = new List<Room>();
        // public List<Entry> Entries = new List<Entry>();

    }
}
