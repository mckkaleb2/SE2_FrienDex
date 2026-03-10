using FrienDex.Data;
using FrienDex.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FrienDex.Services
{
    class RoomRepo : IRoomRepo
    {
        private readonly DexContext _db;

        public RoomRepo(DexContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Creates a new Room in the database and returns the created Room.
        /// </summary>
        /// <param name="newRoom"></param>
        /// <returns>The new room's object</returns>
        public async Task<Room> CreateAsync(Room newRoom)
        {
            var createdRoom = await _db.Rooms.AddAsync(newRoom);
            await _db.SaveChangesAsync();
            return createdRoom.Entity;
        }

        public Task<IEnumerable<Room>> CreateWithListAsync(List<Room> newRooms)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Deletes the room with the specified identifier from the database asynchronously.
        /// </summary>
        /// <remarks>If the room is successfully deleted, changes are saved to the database. Ensure that
        /// the provided <paramref name="id"/> is valid and refers to an existing room.</remarks>
        /// <param name="id">The unique identifier of the room to delete. Must correspond to an existing room in the database.</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">Thrown if no room with the specified <paramref name="id"/> exists in the database.</exception>
        public async Task DeleteAsync(int id)
        {
            var roomToDelete = await _db.Rooms.FindAsync(id);
            if (roomToDelete != null)
            {
                _db.Rooms.Remove(roomToDelete);
                await _db.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Room with id {id} not found.");
            }
        }
        /// <summary>
        /// Asynchronously retrieves a collection of all rooms from the database.
        /// </summary>
        /// <remarks>This method fetches all room records from the database. Ensure that the database
        /// context is properly initialized before calling this method.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of Room objects,
        /// which may be empty if no rooms are found.</returns>
        public async Task<ICollection<Room>> ReadAllAsync()
        {
            var rooms = await _db.Rooms.ToListAsync<Room>();
            return rooms;
        }
        /// <summary>
        /// Asynchronously retrieves a room by its unique identifier.
        /// </summary>
        /// <remarks>This method performs a database lookup to find the room with the specified ID. If the
        /// room does not exist, it returns null.</remarks>
        /// <param name="id">The unique identifier of the room to retrieve. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the room object if found;
        /// otherwise, null.</returns>
        public async Task<Room?> ReadAsync(int id)
        {
            var room = await _db.Rooms.FindAsync(id);
            return room;
        }
        /// <summary>
        /// Updates the details of an existing room identified by its unique identifier.
        /// </summary>
        /// <remarks>This method performs an asynchronous update operation on the room entity. If the room
        /// is not found, a KeyNotFoundException is thrown.</remarks>
        /// <param name="id">The unique identifier of the room to update.</param>
        /// <param name="room">An object containing the updated details of the room, including its name, description, and capacity.</param>
        /// <returns>A task representing the asynchronous operation, with a completion value indicating the number of state
        /// entries written to the database.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if no room with the specified id exists in the database.</exception>
        public Task UpdateAsync(int id, Room room)
        {
            var roomToUpdate = _db.Rooms.Find(id);
            if (roomToUpdate != null)
            {
                roomToUpdate.Name = room.Name;
                roomToUpdate.Description = room.Description;
                roomToUpdate.People = room.People;
                _db.Rooms.Update(roomToUpdate);
                return _db.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Room with id {id} not found.");
            }
        }
    }
}