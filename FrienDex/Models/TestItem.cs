using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace FrienDex.Models
{
    [Table("TestItems")]
    public class TestItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [MaxLength(250), Unique]
        public string Name { get; set; }
        public string Notes { get; set; }
        public bool Done { get; set; }
    }
}
