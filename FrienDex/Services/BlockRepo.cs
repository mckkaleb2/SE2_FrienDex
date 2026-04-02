using FrienDex.Data;
using FrienDex.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrienDex.Services
{
    public class BlockRepo : IBlockRepo
    {
        private readonly DexContext _db;
        private readonly ILogger<Block> _logger;

        public BlockRepo(
            DexContext db,
            ILogger<Block> logger
        )
        {
            _db = db;
            _logger = logger;
        }

        public async Task<Block> CreateAsync(Block newBlock)
        {
            try
            {
                await _db.Blocks.AddAsync(newBlock);
                await _db.SaveChangesAsync();

#if DEBUG
                System.Diagnostics.Debug.WriteLine($"Block created: {newBlock.Type}");
#endif
                return newBlock;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error creating block: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Block>> ReadAllAsync()
        {
            try
            {
                return await _db.Blocks
                    .Include(b => (b as RelationshipBlock)!.RelatedPerson)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error reading all blocks: {ex.Message}");
                return new List<Block>();
            }
        }

        public async Task<Block?> ReadAsync(int id)
        {
            try
            {
                return await _db.Blocks
                    .Include(b => (b as RelationshipBlock)!.RelatedPerson)
                    .FirstOrDefaultAsync(b => b.Id == id);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error reading block {id}: {ex.Message}");
                return null;
            }
        }

        public async Task UpdateAsync(Block block)
        {
            try
            {
                _db.Blocks.Update(block);
                await _db.SaveChangesAsync();

#if DEBUG
                System.Diagnostics.Debug.WriteLine($"Block updated: {block.Id}");
#endif
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating block: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var block = await _db.Blocks.FirstOrDefaultAsync(b => b.Id == id);
                if (block != null)
                {
                    _db.Blocks.Remove(block);
                    await _db.SaveChangesAsync();

#if DEBUG
                    System.Diagnostics.Debug.WriteLine($"Block deleted: {id}");
#endif
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting block: {ex.Message}");
                throw;
            }
        }
    }
}