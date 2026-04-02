using System;
using System.Collections.Generic;
using System.Text;

namespace FrienDex.Data.Entities
{
    public class Person
    {
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the first name of the person.
        /// </summary>
        public string? FirstName { get; set; }
        /// <summary>
        /// Gets or sets the last name of the person.
        /// </summary>
        public string? LastName { get; set; }
        /// <summary>
        /// Gets or sets the IsFavorite property, indicating whether the person is marked as a favorite.
        /// </summary>
        public bool IsFavorite { get; set; } = false;
        /// <summary>
        /// Gets or sets the DexEntry associated with this person. This property may be null if no entry is linked to the person.
        /// </summary>
        public DexEntry? DexEntry { get; set; }
        /// <summary>
        /// Gets or sets the collection of rooms associated with this person. This collection can be modified to add or remove rooms as needed. Each room represents a distinct space or context in which the person is involved.
        /// </summary>
        public List<Room> Rooms { get; set; } = new List<Room>();

        public string FullName => $"{FirstName} {LastName}".Trim();

        public override string ToString()
        {
            string roomRepeater = "";
            
            foreach (Room r in Rooms)
            {
                roomRepeater += "\n";
                roomRepeater += r.ToString();
                roomRepeater += "\n\t_-_-_-_-_-_";
            }

            string formatter =
                $"PERSON - {Id}"
                +$"\n\tP- \'{FirstName}\' \'{LastName}\'"
                +$"\n\tP- IsFavorite: {IsFavorite}"
                +$"\n\tP- DexEntry: \n\t----{(DexEntry != null ? DexEntry.ToString() : "null")}\n\t----"
                +$"\n\tP- Rooms: \n\t----{(Rooms.Count > 0 ? roomRepeater : "None yet")}\n\t----";
                //+$"\n\tRooms: \n\t----{(Rooms != null ? string.Join(", ", Rooms.Select(r => r.Id).ToString()) : "null")}\n\t----";
            return formatter;
            //return base.ToString();
        }
    }
}
