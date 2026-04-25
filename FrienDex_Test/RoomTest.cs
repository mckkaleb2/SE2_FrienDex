using FrienDex.Data;
using FrienDex.Data.Entities;
using FrienDex.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace FrienDex_Test
{
    public class RoomTest
    {
        private readonly DexContext _db;

        public RoomTest()
        {
            // Setup in-memory database
            var options = new DbContextOptionsBuilder<DexContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _db = new DexContext(options);
        }

        // CREATE TEST
        [Fact]
        public async Task TestCreateRoomSuccess()
        {
            // ARRANGE
            RoomRepo roomRepo = new RoomRepo(_db);

            // Creates a Person object for the test
            Room testRoom = new Room
            {
                Name = "Pewter City Gym",
                Description = "The home of Pewter City's gym leader: Brock"
            };

            // ACT
            // Calls the CreatePerson operation
            await roomRepo.CreateAsync(testRoom);

            // Reads the database for the created Person object to see if it was successfully created
            Room? result = await roomRepo.ReadAsync(testRoom.Id);

            // ASSERT
            Assert.NotNull(result);
            Assert.Equal("Pewter City Gym", result.Name);
            Assert.Equal("The home of Pewter City's gym leader: Brock", result.Description);
        }

        // READ TEST


        // READ ALL TEST


        // UPDATE TEST


        // DELETE TEST
    }
}
