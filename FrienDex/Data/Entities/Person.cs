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
        public bool? IsFavorite { get; set; }
        /// <summary>
        /// Gets or sets the DexEntry associated with this person. This property may be null if no entry is linked to the person.
        /// </summary>
        public DexEntry? DexEntry { get; set; }
        /// <summary>
        /// Gets or sets the collection of rooms associated with this person. This collection can be modified to add or remove rooms as needed. Each room represents a distinct space or context in which the person is involved.
        /// </summary>
        public List<Room> Rooms { get; set; } = new List<Room>();

        public override string ToString()
        {
            string formatter =
                $"PERSON - {Id}"
                +$"\n\t\'{FirstName}\' \'{LastName}\'"
                +$"\n\tIsFavorite: {IsFavorite}"
                +$"\n\tDexEntry: {(DexEntry != null ? DexEntry.Id.ToString() : "null")}"
                +$"\n\tRooms: {(Rooms != null ? string.Join(", ", Rooms.Select(r => r.Id)) : "null")}";
            return formatter;
            //return base.ToString();
        }
    }
}
