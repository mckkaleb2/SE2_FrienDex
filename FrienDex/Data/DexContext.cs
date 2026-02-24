using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace FrienDex.Data
{
    public class DexContext : DbContext    
    {
        public DbSet<TestItem> TestItems { get; set; }
        private readonly string _Path;
        public DexContext()
        {
            _Path = Path.Combine(FileSystem.AppDataDirectory, "dex.db");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={_Path}");
        }
    }
}
