using System;
using System.Collections.Generic;
using System.Text;
using FrienDex.Data.Entities;

namespace FrienDex.Services
{
    public interface IRoomRepo
    {
            // CREATE
            Task<Room> CreateAsync(Room newRoom);
    
            // CREATE with Lists
            Task<IEnumerable<Room>> CreateWithListAsync(List<Room> newRooms);
    
            // READALL
            Task<ICollection<Room>> ReadAllAsync();
    
            // READ
            Task<Room?> ReadAsync(int id);
    
            // UPDATE
            Task UpdateAsync(int id,  Room room);
    
            // DELETE
            Task DeleteAsync(int id);
    }
}
