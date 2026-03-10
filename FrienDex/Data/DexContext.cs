using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
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
            System.Diagnostics.Debug.WriteLine("\n\t\tSETTING A NEW ONCONFIGURING\n\n");
            optionsBuilder
                .UseSqlite($"Filename={_Path}")
                // TODO: Create a proper logger that we can use for LogTo()
                .LogTo(Console.WriteLine) // Log EF Core Evets to the console
                                          //.LogTo(System.Diagnostics.Debug.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information) // Log EF Core Evets to the console
                                          // Alternatively, we could use .UseAsyncSeeding() for better performance, but .UseSeeding() is simpler for now.
                                          // .UseAsyncSeeding(async (context, _, cancellationToken) =>
#if DEBUG
                .UseSeeding((context, _) =>
                {

                    string logPrefix = "\n\n---------------- SEED DATA SYNC --- log ---------\n\n";
                    string logSuffix = "\n\n----- END ------ SEED DATA SYNC --- log ---------\n\n";
                    string linePrefix = "\n\t>\t-- ";

                    System.Diagnostics.Debug.WriteLine(logPrefix);
                    //Cons ole.WriteLine(logPrefix);
                    Person personLoader = new Person { Id = 1, FirstName = "Alice" };
                    DexEntry dexLoader = new DexEntry { Id = 1, PersonId = 1 , Person = personLoader};
                    personLoader.DexEntry = dexLoader;

                    #region seedPeopleSync
                    var testPerson1 = context.Set<Person>().FirstOrDefault(p => p.Id == 1);
                    if (testPerson1 == null)
                    {
                        //context.Set<Person>().Add(new Person { Id = 1, FirstName = "Alice" });
                        context.Set<Person>().Add(personLoader);
                        context.SaveChanges();
                        testPerson1 = context.Set<Person>().FirstOrDefault(p => p.Id == 1);
                        System.Diagnostics.Debug.Write($"{linePrefix}\t\tAdded Person 1 to the Database\n\n");
                    }
                    else System.Diagnostics.Debug.Write($"{linePrefix}\t\tNOTE:\t\tPerson 1 already exists in the database.\n\n");


                    var testPerson2 = context.Set<Person>().FirstOrDefault(p => p.Id == 2);
                    if (testPerson2 == null)
                    {
                        context.Set<Person>().Add(new Person { Id = 2, FirstName = "Bob" });
                        context.SaveChanges();
                        testPerson2 = context.Set<Person>().FirstOrDefault(p => p.Id == 2);
                        System.Diagnostics.Debug.Write($"{linePrefix}\t\tAdded Person 2 to the Database\n\n");
                    }
                    else System.Diagnostics.Debug.Write($"{linePrefix}\t\tNOTE:\t\tPerson 2 already exists in the database.\n\n");
                    #endregion seedPeopleSync

                    #region seedEntriesSync
                    var testDexEntry1 = context.Set<DexEntry>().FirstOrDefault(e => e.Id == 1);
                    if (testDexEntry1 == null)
                    {
                        context.Set<DexEntry>().Add(testPerson1!.DexEntry);
                        context.SaveChanges();
                        testDexEntry1 = context.Set<DexEntry>().FirstOrDefault(e => e.Id == 1);
                        System.Diagnostics.Debug.Write($"{linePrefix}\t\tPerson 1's DexEntry added to in the database.\n\n");
                    }
                    else System.Diagnostics.Debug.Write($"{linePrefix}\t\tPerson 1's DexEntry already exists in the database.\n\n");

                    var testDexEntry2 = context.Set<DexEntry>().FirstOrDefault(p => p.Id == 2);
                    if (testDexEntry2 == null)
                    {
                        testPerson2!.DexEntry = new DexEntry { Id = 2, PersonId = 2, Person = testPerson2! };

                        context.Set<DexEntry>().Add(new DexEntry { Id = 2, PersonId = 2, Person = testPerson2! });
                        context.SaveChanges();
                        testDexEntry2 = context.Set<DexEntry>().FirstOrDefault(p => p.Id == 2)
                            ?? throw new Exception("Failed to retrieve testDexEntry2 after adding it to the database.");
                        System.Diagnostics.Debug.Write($"{linePrefix}\t\tPerson 2's DexEntry added to in the database.\n\n");
                    }
                    else System.Diagnostics.Debug.Write($"{linePrefix}\t\tPerson 2's DexEntry already exists in the database.\n\n");
                    #endregion seedEntriesSync

                    #region seedRoomsSync
                    var testRoom1 = context.Set<Room>().FirstOrDefault(e => e.Id == 1);
                    if (testRoom1 == null)
                    {
                        context.Set<Room>().Add(new Room { Id = 1, Name = "Room A", Description = "This is a test room that was added during seeding" });
                        context.SaveChanges();
                        testRoom1 = context.Set<Room>().FirstOrDefault(e => e.Id == 1);
                        System.Diagnostics.Debug.Write($"{linePrefix}\t\tAdded Room A to the Database\n\n");
                    }
                    else System.Diagnostics.Debug.Write($"{linePrefix}\t\tRoom A already exists in the database.\n\n");

                    var testRoom2 = context.Set<Room>().FirstOrDefault(p => p.Id == 2);
                    if (testRoom2 == null)
                    {
                        context.Set<Room>().Add(new Room { Id = 2, Name = "Room B", Description = "This is a test room that was added during seeding" });
                        context.SaveChanges();
                        testRoom2 = context.Set<Room>().FirstOrDefault(p => p.Id == 2);
                        System.Diagnostics.Debug.Write($"{linePrefix}\t\tAdded Room 2 to the Database\n\n");
                    }
                    else System.Diagnostics.Debug.Write($"{linePrefix}\t\tRoom 2 already exists in the database.\n\n");
                    #endregion seedRoomsSync


                    if (!testRoom1!.People.Any(p => p.Id == testPerson1!.Id))
                    {
                        testRoom1.People.Add(testPerson1!);
                        context.SaveChanges();
                        System.Diagnostics.Debug.Write($"{linePrefix}\t\tAdded Room A to Person 1\n\n");
                        if (!testPerson1!.Rooms.Any(r => r.Id == testRoom1.Id))
                        {
                            testPerson1.Rooms.Add(testRoom1);
                            context.SaveChanges();
                            System.Diagnostics.Debug.Write($"{linePrefix}\t\tAdded Person 1 to Room A\n\n");
                        }
                        else System.Diagnostics.Debug.Write($"{linePrefix}\t\tNOTE:\t\tPerson 1 already has Room A in their Rooms collection.\n\n");
                    }
                    else System.Diagnostics.Debug.Write($"{linePrefix}\t\tNOTE:\t\tRoom A already has Person 1 in its People collection.\n\n");

                    if (!testRoom2!.People.Any(p => p.Id == testPerson1!.Id))
                    {
                        testRoom2.People.Add(testPerson1!);
                        context.SaveChanges();
                        System.Diagnostics.Debug.Write($"{linePrefix}\t\tAdded Room B to Person 1\n\n");
                        if (!testPerson1!.Rooms.Any(r => r.Id == testRoom2.Id))
                        {
                            testPerson1.Rooms.Add(testRoom2);
                            context.SaveChanges();
                            System.Diagnostics.Debug.Write($"{linePrefix}\t\tAdded Person 1 to Room B\n\n");
                        }
                        else System.Diagnostics.Debug.Write($"{linePrefix}\t\tNOTE:\t\tPerson 1 already has Room B in their Rooms collection.\n\n");
                    }
                    else System.Diagnostics.Debug.Write($"{linePrefix}\t\tNOTE:\t\tRoom B already has Person 1 in its People collection.\n\n");


                    if (!testRoom2!.People.Any(p => p.Id == testPerson2!.Id))
                    {
                        testRoom2.People.Add(testPerson2!);
                        context.SaveChanges();
                        System.Diagnostics.Debug.Write($"{linePrefix}\t\tAdded Room B to Person 2\n\n");
                        if (!testPerson2!.Rooms.Any(r => r.Id == testRoom2.Id))
                        {
                            testPerson2.Rooms.Add(testRoom2);
                            context.SaveChanges();
                            System.Diagnostics.Debug.Write($"{linePrefix}\t\tAdded Person 2 to Room B\n\n");
                        }
                        else System.Diagnostics.Debug.Write($"{linePrefix}\t\tNOTE:\t\tPerson 2 already has Room B in their Rooms collection.\n\n");
                    }
                    else System.Diagnostics.Debug.Write($"{linePrefix}\t\tNOTE:\t\tRoom B already has Person 2 in its People collection.\n\n");

                    System.Diagnostics.Debug.WriteLine(logSuffix);

                }) // END .UseSeeding
#endif
#if DEBUG
                .UseAsyncSeeding(async (context, _, cancellationToken) =>
                {

                    string logPrefix = "\n\n---------------- SEED DATA ASYNCRONOUS --- log ---------\n\n";
                    string logSuffix = "\n\n----- END ------ SEED DATA ASYNCRONOUS --- log ---------\n\n";
                    string linePrefix = "\n\t>\t-- ";

                    System.Diagnostics.Debug.WriteLine(logPrefix);

                    #region seedPeopleAsyncronous
                    var testPerson1 = await context.Set<Person>().FirstOrDefaultAsync(p => p.Id == 1);
                    if (testPerson1 == null)
                    {
                        context.Set<Person>().Add(new Person { Id = 1, FirstName = "Alice" });
                        await context.SaveChangesAsync(cancellationToken);
                        testPerson1 = await context.Set<Person>().FirstOrDefaultAsync(p => p.Id == 1);
                        System.Diagnostics.Debug.Write($"{linePrefix}\t\tAdded Person 1 to the Database\n\n");
                    }
                    else System.Diagnostics.Debug.Write($"{linePrefix}\t\tNOTE:\t\tPerson 1 already exists in the database.\n\n");


                    var testPerson2 = await context.Set<Person>().FirstOrDefaultAsync(p => p.Id == 2);
                    if (testPerson2 == null)
                    {
                        context.Set<Person>().Add(new Person { Id = 2, FirstName = "Bob" });
                        await context.SaveChangesAsync(cancellationToken);
                        testPerson2 = await context.Set<Person>().FirstOrDefaultAsync(p => p.Id == 2);
                        System.Diagnostics.Debug.Write($"{linePrefix}\t\tAdded Person 2 to the Database\n\n");
                    }
                    else System.Diagnostics.Debug.Write($"{linePrefix}\t\tNOTE:\t\tPerson 2 already exists in the database.\n\n");
                    #endregion seedPeopleAsyncronous

                    #region seedEntriesAsyncronous
                    var testDexEntry1 = await context.Set<DexEntry>().FirstOrDefaultAsync(e => e.Id == 1);
                    if (testDexEntry1 == null)
                    {
                        context.Set<DexEntry>().Add(new DexEntry { Id = 1, PersonId = 1, Person = testPerson1! });
                        await context.SaveChangesAsync(cancellationToken);
                        testDexEntry1 = await context.Set<DexEntry>().FirstOrDefaultAsync(e => e.Id == 1);
                        System.Diagnostics.Debug.Write($"{linePrefix}\t\tPerson 1's DexEntry added to in the database.\n\n");
                    }
                    else System.Diagnostics.Debug.Write($"{linePrefix}\t\tPerson 1's DexEntry already exists in the database.\n\n");

                    var testDexEntry2 = await context.Set<DexEntry>().FirstOrDefaultAsync(p => p.Id == 2);
                    if (testDexEntry2 == null)
                    {
                        context.Set<DexEntry>().Add(new DexEntry { Id = 2, PersonId = 2, Person = testPerson2! });
                        await context.SaveChangesAsync(cancellationToken);
                        testDexEntry2 = await context.Set<DexEntry>().FirstOrDefaultAsync(p => p.Id == 2)
                            ?? throw new Exception("Failed to retrieve testDexEntry2 after adding it to the database.");
                        System.Diagnostics.Debug.Write($"{linePrefix}\t\tPerson 2's DexEntry added to in the database.\n\n");
                    }
                    else System.Diagnostics.Debug.Write($"{linePrefix}\t\tPerson 2's DexEntry already exists in the database.\n\n");
                    #endregion seedEntriesAsyncronous

                    #region seedRoomsAsyncronous
                    var testRoom1 = await context.Set<Room>().FirstOrDefaultAsync(e => e.Id == 1);
                    if (testRoom1 == null)
                    {
                        context.Set<Room>().Add(new Room { Id = 1, Name = "Room A", Description = "This is a test room that was added during seeding" });
                        await context.SaveChangesAsync(cancellationToken);
                        testRoom1 = await context.Set<Room>().FirstOrDefaultAsync(e => e.Id == 1);
                        System.Diagnostics.Debug.Write($"{linePrefix}\t\tAdded Room A to the Database\n\n");
                    }
                    else System.Diagnostics.Debug.Write($"{linePrefix}\t\tRoom A already exists in the database.\n\n");

                    var testRoom2 = await context.Set<Room>().FirstOrDefaultAsync(p => p.Id == 2);
                    if (testRoom2 == null)
                    {
                        context.Set<Room>().Add(new Room { Id = 2, Name = "Room B", Description = "This is a test room that was added during seeding" });
                        await context.SaveChangesAsync(cancellationToken);
                        testRoom2 = await context.Set<Room>().FirstOrDefaultAsync(p => p.Id == 2);
                        System.Diagnostics.Debug.Write($"{linePrefix}\t\tAdded Room 2 to the Database\n\n");
                    }
                    else System.Diagnostics.Debug.Write($"{linePrefix}\t\tRoom 2 already exists in the database.\n\n");
                    #endregion seedRoomsAsyncronous


                    if (!testRoom1!.People.Any(p => p.Id == testPerson1!.Id))
                    {
                        testRoom1.People.Add(testPerson1!);
                        await context.SaveChangesAsync(cancellationToken);
                        System.Diagnostics.Debug.Write($"{linePrefix}\t\tAdded Room A to Person 1\n\n");
                        if (!testPerson1!.Rooms.Any(r => r.Id == testRoom1.Id))
                        {
                            testPerson1.Rooms.Add(testRoom1);
                            await context.SaveChangesAsync(cancellationToken);
                            System.Diagnostics.Debug.Write($"{linePrefix}\t\tAdded Person 1 to Room A\n\n");
                        }
                        else System.Diagnostics.Debug.Write($"{linePrefix}\t\tNOTE:\t\tPerson 1 already has Room A in their Rooms collection.\n\n");
                    }
                    else System.Diagnostics.Debug.Write($"{linePrefix}\t\tNOTE:\t\tRoom A already has Person 1 in its People collection.\n\n");


                    if (!testRoom2!.People.Any(p => p.Id == testPerson1!.Id))
                    {
                        testRoom2.People.Add(testPerson1!);
                        await context.SaveChangesAsync(cancellationToken);
                        System.Diagnostics.Debug.Write($"{linePrefix}\t\tAdded Room B to Person 1\n\n");
                        if (!testPerson1!.Rooms.Any(r => r.Id == testRoom2.Id))
                        {
                            testPerson1.Rooms.Add(testRoom2);
                            await context.SaveChangesAsync(cancellationToken);
                            System.Diagnostics.Debug.Write($"{linePrefix}\t\tAdded Person 1 to Room B\n\n");
                        }
                        else System.Diagnostics.Debug.Write($"{linePrefix}\t\tNOTE:\t\tPerson 1 already has Room B in their Rooms collection.\n\n");
                    }
                    else System.Diagnostics.Debug.Write($"{linePrefix}\t\tNOTE:\t\tRoom B already has Person 1 in its People collection.\n\n");


                    if (!testRoom2!.People.Any(p => p.Id == testPerson2!.Id))
                    {
                        testRoom2.People.Add(testPerson2!);
                        await context.SaveChangesAsync(cancellationToken);
                        System.Diagnostics.Debug.Write($"{linePrefix}\t\tAdded Room B to Person 2\n\n");
                        if (!testPerson2!.Rooms.Any(r => r.Id == testRoom2.Id))
                        {
                            testPerson2.Rooms.Add(testRoom2);
                            await context.SaveChangesAsync(cancellationToken);
                            System.Diagnostics.Debug.Write($"{linePrefix}\t\tAdded Person 2 to Room B\n\n");
                        }
                        else System.Diagnostics.Debug.Write($"{linePrefix}\t\tNOTE:\t\tPerson 2 already has Room B in their Rooms collection.\n\n");
                    }
                    else System.Diagnostics.Debug.Write($"{linePrefix}\t\tNOTE:\t\tRoom B already has Person 2 in its People collection.\n\n");

                    System.Diagnostics.Debug.WriteLine(logSuffix);
                }) // END .UseAsyncSeeding
#endif
            ;
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
