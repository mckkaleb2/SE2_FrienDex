using FrienDex.Data.Entities;

namespace FrienDex.Services
{
    public interface IBlockRepo
    {
        // CREATE
        Task<Block> CreateAsync(Block newBlock);

        // READALL
        Task<List<Block>> ReadAllAsync();

        // READ
        Task<Block?> ReadAsync(int id);

        // UPDATE
        Task UpdateAsync(Block block);

        // DELETE
        Task DeleteAsync(int id);
    }
}