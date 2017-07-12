using Pets.Core.Repositories;
using System;
using System.Collections.Generic;
using Pets.Core.Domain;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Pets.Infrastructure.Repositories
{
    public class InMemoryAnimalRepository : DbContext, IAnimalRepository
    {
        DbSet<Animal> _animals { get; set; }

        public InMemoryAnimalRepository(DbContextOptions<InMemoryAnimalRepository> options)
            : base(options)
        {
        }

        public async Task<Animal> GetAsync(Guid id)
        {
            return await Task.FromResult(_animals.SingleOrDefault(x => x.Id == id));
        }

        public async Task<Animal> GetAsync(Guid userId, string name)
        {
            return await Task.FromResult(_animals.Where(x => x.UserId == userId && x.Name.Equals(name)).FirstOrDefault());
        }

        public async Task<IEnumerable<Animal>> GetAllAsync(Guid userId)
        {
            return await Task.FromResult(_animals.Where(x => x.UserId == userId).ToList());
        }

        public async Task AddAsync(Animal animal)
        {
            await _animals.AddAsync(animal);
            await SaveChangesAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            var animal = await GetAsync(id);
            _animals.Remove(animal);

            await SaveChangesAsync();
        }

        public async Task UpdateAsync(Animal animal)
        {
            _animals.Update(animal);

            await SaveChangesAsync();
        }
    }
}
