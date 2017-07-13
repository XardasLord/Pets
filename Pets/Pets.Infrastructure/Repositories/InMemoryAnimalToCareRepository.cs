using Pets.Core.Repositories;
using System;
using System.Collections.Generic;
using Pets.Core.Domain;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Pets.Infrastructure.Repositories
{
    public class InMemoryAnimalToCareRepository : DbContext, IAnimalToCareRepository
    {
        DbSet<AnimalToCare> _animalsToCare { get; set; }

        public InMemoryAnimalToCareRepository(DbContextOptions<InMemoryAnimalToCareRepository> options)
            : base(options)
        {
        }

        public async Task AddAsync(AnimalToCare animalToCare)
        {
            await _animalsToCare.AddAsync(animalToCare);
            await SaveChangesAsync();
        }

        public async Task<IEnumerable<AnimalToCare>> GetAllActiveAsync()
        {
            return await Task.FromResult(_animalsToCare.Where(x => x.IsTaken == false && x.DateFrom >= DateTime.UtcNow).ToList());
        }

        public async Task<IEnumerable<AnimalToCare>> GetAllArchiveAsync()
        {
            return await Task.FromResult(_animalsToCare.Where(x => x.IsTaken == true || x.DateFrom < DateTime.UtcNow).ToList());
        }

        public async Task<IEnumerable<AnimalToCare>> GetAsync(Guid animalId)
        {
            return await Task.FromResult(_animalsToCare.Where(x => x.AnimalId == animalId).ToList());
        }

        public async Task RemoveAsync(AnimalToCare animalToCare)
        {
            _animalsToCare.Remove(animalToCare);
            await SaveChangesAsync();
        }

        public async Task UpdateAsync(AnimalToCare animalToCare)
        {
            _animalsToCare.Update(animalToCare);
            await SaveChangesAsync();
        }
    }
}
