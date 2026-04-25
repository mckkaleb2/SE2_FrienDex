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


        // CREATE TESTS
        /// <summary>
        /// Tests the CreatePersonAsync operation.
        /// Creates a new Person object, calls the CreatePerson operation,
        /// tests if the object was created.
        /// Should result in all test conditions being true.
        /// </summary>
        [Fact]
        public async Task TestCreatePersonSuccess()
        {
            // ARRANGE
            PersonRepo personRepo = new PersonRepo(_db, _logger);

            // Creates a Person object for the test
            Person testPerson = new Person
            {
                FirstName = "Rival",
                LastName = "Blue",
                IsFavorite = false
            };

            // ACT
            // Calls the CreatePerson operation
            await personRepo.CreateAsync(testPerson);

            // Reads the database for the created Person object to see if it was successfully created
            Person? result = await personRepo.ReadAsync(testPerson.Id);

            // ASSERT
            Assert.NotNull(result);
            Assert.Equal("Rival", result.FirstName);
            Assert.Equal("Blue", result.LastName);
            Assert.False(result.IsFavorite);
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
            Assert.True(result.IsFavorite);
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

        // READ ALL TESTS

        /// <summary>
        /// Tests the ReadAllAsync operation.
        /// Creates a few Person objects, adds them to the database, 
        /// then attempts to ReadAll.
        /// </summary>
        [Fact]
        public async Task TestReadAllSuccess()
        {
            // ARRANGE
            PersonRepo personRepo = new PersonRepo(_db, _logger);

            Person testPerson1 = new Person
            {
                FirstName = "Trainer",
                LastName = "Leaf",
                IsFavorite = true
            };

            Person testPerson2 = new Person
            {
                FirstName = "Trainer",
                LastName = "Yellow"
            };

            Person testPerson3 = new Person
            {
                FirstName = "Trainer",
                LastName = "Green",
                IsFavorite = false
            };

            await _db.People.AddAsync(testPerson1);
            await _db.People.AddAsync(testPerson2);
            await _db.People.AddAsync(testPerson3);
            await _db.SaveChangesAsync();

            // ACT
            List<Person> resultList = await personRepo.ReadAllAsync();

            // ASSERT
            Assert.NotEmpty(resultList);
            Assert.Equal("Trainer", resultList[(testPerson1.Id - 1)].FirstName);
            Assert.Equal("Leaf", resultList[(testPerson1.Id - 1)].LastName);
            Assert.True(resultList[(testPerson1.Id - 1)].IsFavorite);
            Assert.Equal("Trainer", resultList[(testPerson2.Id - 1)].FirstName);
            Assert.Equal("Yellow", resultList[(testPerson2.Id - 1)].LastName);
            Assert.False(resultList[(testPerson2.Id - 1)].IsFavorite);
            Assert.Equal("Trainer", resultList[(testPerson3.Id - 1)].FirstName);
            Assert.Equal("Green", resultList[(testPerson3.Id - 1)].LastName);
            Assert.False(resultList[(testPerson3.Id - 1)].IsFavorite);
        }

        /// <summary>
        /// Tests for a case where the object reference is out of range.
        /// Should result in an Exception.
        /// </summary>
        [Fact]
        public async Task TestReadAllNotInRangeFailure()
        {
            // ARRANGE
            PersonRepo personRepo = new PersonRepo(_db, _logger);

            Person testPerson1 = new Person
            {
                FirstName = "Trainer",
                LastName = "Leaf",
                IsFavorite = true
            };

            await _db.People.AddAsync(testPerson1);
            await _db.SaveChangesAsync();

            // ACT
            List<Person> resultList = await personRepo.ReadAllAsync();

            // ASSERT
            Assert.NotEmpty(resultList);
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => resultList[testPerson1.Id]);
        }

        /// <summary>
        /// Tests the UpdateAsync for the Person object.
        /// Creates a new Person object, calls the UpdateAsync method with new Person data, 
        /// then reads the Person object to check if the changes were made.
        /// </summary>
        [Fact]
        public async Task TestUpdatePersonSuccess()
        {
            // ARRANGE
            PersonRepo personRepo = new PersonRepo(_db, _logger);

            Person testPerson1 = new Person
            {
                FirstName = "Professor",
                LastName = "Oak",
                IsFavorite = true
            };

            await _db.People.AddAsync(testPerson1);
            await _db.SaveChangesAsync();

            Person newPerson = new Person
            {
                FirstName = "Professor",
                LastName = "Elm",
                IsFavorite = true
            };

            // ACT
            await personRepo.UpdateAsync(testPerson1.Id, newPerson);

            // ASSERT
            Assert.Equal("Professor", testPerson1.FirstName);
            Assert.Equal("Elm", testPerson1.LastName);
            Assert.True(testPerson1.IsFavorite);
        }

        [Fact]
        public async Task TestDeletePersonSuccess()
        {
            // ARRANGE
            PersonRepo personRepo = new PersonRepo(_db, _logger);

            Person testPerson1 = new Person
            {
                FirstName = "Professor",
                LastName = "Sada",
                IsFavorite = false
            };

            await _db.People.AddAsync(testPerson1);
            await _db.SaveChangesAsync();

            // ACT
            await personRepo.DeleteAsync(testPerson1.Id);

            Person? result = await personRepo.ReadAsync(testPerson1.Id);

            // ASSERT
            Assert.Null(result);
        }


    }
}
