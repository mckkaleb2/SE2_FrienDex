using System;
using System.Collections.Generic;
using System.Text;

namespace FrienDex.Data.Entities
{
    public class Room
    {
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the name associated with the entity.
        /// </summary>
        /// <remarks>The name is a required field and cannot be null or empty. It is used to identify the
        /// entity in various contexts.</remarks>
        public required string Name { get; set; }
        /// <summary>
        /// Gets or sets the description associated with the entity.
        /// </summary>
        /// <remarks>The description field is required and cannot be null or empty.</remarks>
        public required string Description { get; set; }
        /// <summary>
        /// Gets or sets the collection of people associated with this entity.
        /// </summary>
        /// <remarks>The People property provides access to a list of Person objects, enabling management
        /// of multiple individuals related to the entity. The collection is initialized to an empty list when the
        /// property is created.</remarks>
        public List<Person> People { get; set; } = new List<Person>();

        public override string ToString()
        {
            string formatter =
                $"Room - {Id}"
                + $"\n\tR- Name: {Name}"
                + $"\n\tR- Description: {Description}"
                + $"\n\tR- People: {(People != null ? string.Join(", ", People.Select(p => p.Id)) : "null")}";
            return formatter;
            //return base.ToString();
        }

    }
}
