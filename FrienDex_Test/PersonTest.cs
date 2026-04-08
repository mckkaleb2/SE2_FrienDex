//using AndroidX.Core.Provider;
using FrienDex.Data;
using FrienDex.Data.Entities;
using FrienDex.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
//using Java.Lang;

namespace FrienDex_Test
{
    public class PersonTest
    {
        private readonly DexContext _db;
        private readonly ILogger<Person> _logger;

        public PersonTest()
        {
            // Setup in-memory database
            var options = new DbContextOptionsBuilder<DexContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _db = new DexContext(options);

            // Setup logger (no-op logger)
            _logger = NullLogger<Person>.Instance;
        }


        // READ TESTS
        /// <summary>
        /// Tests the ReadPersonAsync operation. 
        /// Creates a new Person object and then attempts to read it.
        /// Should result in all of the set tests being equal to the result's parameters.
        /// </summary>
        [Fact]
        public async Task TestReadPersonSuccess()
        {
            // ARRANGE
            PersonRepo personRepo = new PersonRepo(_db, _logger);

            // Creates a Person object for the purposes of testing
            Person testPerson = new Person
            {
                FirstName = "Ash",
                LastName = "Ketcham",
                IsFavorite = true
            };

            await _db.People.AddAsync(testPerson);
            await _db.SaveChangesAsync();

            // ACT
            // Reads the test Person object 
            Person? result = await personRepo.ReadAsync(testPerson.Id);

            // ASSERT
            Assert.NotNull(result);
            Assert.Equal("Ash", result.FirstName);
            Assert.Equal("Ketcham", result.LastName);
            Assert.Equal(true, result.IsFavorite);
        }

        /// <summary>
        /// Tests for the case of an object that is not the intended target being called.
        /// The object called may have similar data (i.e., IsFavorite could be the same), but the IDs will not match.
        /// Should result in the wrong object being called.
        /// </summary>
        [Fact]
        public async Task TestReadPersonWrongReferenceFailure()
        {
            // ARRANGE
            PersonRepo personRepo = new PersonRepo(_db, _logger);

            Person testPerson1 = new Person
            {
                FirstName = "Ash",
                LastName = "Ketcham",
                IsFavorite = true
            };

            Person testPerson2 = new Person
            {
                FirstName = "Gary",
                LastName = "Oak",
                IsFavorite = false
            };

            await _db.People.AddAsync(testPerson1);
            await _db.People.AddAsync(testPerson2);
            await _db.SaveChangesAsync();

            // ACT
            Person? result = await personRepo.ReadAsync(testPerson2.Id);

            // ASSERT
            Assert.NotNull(result);
            Assert.NotEqual(testPerson1.Id, result.Id);
        }

        /// <summary>
        /// Tests for the case of an object that does not exist is called.
        /// Should result in a NULL object.
        /// </summary>
        [Fact]
        public async Task TestReadPersonNoReferenceFailure()
        {
            // ARRANGE
            PersonRepo personRepo = new PersonRepo(_db, _logger);

            // ACT
            Person? result = await personRepo.ReadAsync(935);

            // ASSERT
            Assert.Null(result);
        }
    }
}
