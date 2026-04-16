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
        private readonly ILogger<Room> _logger;

        public RoomTest()
        {
            // Setup in-memory database
            var options = new DbContextOptionsBuilder<DexContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _db = new DexContext(options);

            // Setup logger (no-op logger)
            _logger = NullLogger<Room>.Instance;
        }

        // CREATE TEST


        // READ TEST

        
        // READ ALL TEST


        // UPDATE TEST


        // DELETE TEST
    }
}
