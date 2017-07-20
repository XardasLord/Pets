using Pets.Core.Domain;
using Pets.Core.Repositories;
using Pets.Infrastructure.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pets.Infrastructure.Repositories
{
    public class AnimalToCareRepository : IAnimalToCareRepository
    {
        private readonly PetsContext _context;

        public AnimalToCareRepository(PetsContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AnimalToCare animalToCare)
        {
            await _context.AnimalsToCare.AddAsync(animalToCare);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AnimalToCare>> GetAllActiveAsync()
        {
            return await Task.FromResult(_context.AnimalsToCare.Where(x => x.IsTaken == false && x.DateFrom >= DateTime.UtcNow).ToList());
        }

        public async Task<IEnumerable<AnimalToCare>> GetAllArchiveAsync()
        {
            return await Task.FromResult(_context.AnimalsToCare.Where(x => x.IsTaken == true || x.DateFrom < DateTime.UtcNow).ToList());
        }

        public async Task<IEnumerable<AnimalToCare>> GetAsync(Guid animalId)
        {
            return await Task.FromResult(_context.AnimalsToCare.Where(x => x.AnimalId == animalId).ToList());
        }

        public async Task<IEnumerable<AnimalToCare>> GetAnimalsCaringByUserAsync(Guid userId)
        {
            return await Task.FromResult(
                _context.AnimalsToCare
                .Where(x => x.IsTaken == true && x.UserId == userId && x.DateTo >= DateTime.UtcNow)
                .ToList());
        }

        public async Task<IEnumerable<AnimalToCare>> GetAnimalsCaringByUserArchiveAsync(Guid userId)
        {
            return await Task.FromResult(
                _context.AnimalsToCare
                .Where(x => x.IsTaken == true && x.UserId == userId && x.DateTo < DateTime.UtcNow)
                .ToList());
        }

        public async Task RemoveAsync(AnimalToCare animalToCare)
        {
            _context.AnimalsToCare.Remove(animalToCare);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AnimalToCare animalToCare)
        {
            _context.AnimalsToCare.Update(animalToCare);
            await _context.SaveChangesAsync();
        }
    }
}
