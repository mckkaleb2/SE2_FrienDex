using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FrienDex.Data.Entities
{
    // Class name changed from Entry to DexEntry to avoid conflicts with System.EntryPointNotFoundException
    public class DexEntry
    {
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the person associated with this entry.
        /// </summary>
        /// <remarks>Assigning a null value to this property may result in a null reference exception when
        /// accessing the person's details. Ensure that a valid Person object is provided.</remarks>
        public required Person Person { get; set; }
        [ForeignKey("Person")]
        public int PersonId { get; set; }
        /// <summary>
        /// Gets or sets the collection of blocks associated with this instance.
        /// </summary>
        /// <remarks>The collection can be modified to add or remove blocks as needed. Each block
        /// represents a distinct unit of functionality or data within the context of this instance.</remarks>
        public List<Block> Blocks { get; set; } = new List<Block>();
    }
}
