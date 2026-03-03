using FrienDex.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FrienDex.Data
{
    public class DexContext : DbContext
    {
        public DbSet<Person> People { get; set; }
        public DbSet<DexEntry> Entries { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Block> Blocks { get; set; }
        private readonly string _Path;
        public DexContext()
        {
            _Path = Path.Combine(FileSystem.AppDataDirectory, "dex.db");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={_Path}");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DexEntry>()
                .HasOne(d => d.Person)
                .WithOne(p => p.DexEntry)
                .HasForeignKey<DexEntry>(d => d.PersonId)
                .IsRequired();

            modelBuilder.Entity<Block>()
                .HasDiscriminator<BlockType>("BlockType")
                .HasValue<TextBlock>(BlockType.TextBlock)
                .HasValue<ImageBlock>(BlockType.ImageBlock)
                .HasValue<DatePickerBlock>(BlockType.DatePickerBlock)
                .HasValue<EventBlock>(BlockType.EventBlock)
                .HasValue<RelationshipBlock>(BlockType.RelationshipBlock)
                .HasValue<ContactBlock>(BlockType.ContactBlock);
        }
    }
}
