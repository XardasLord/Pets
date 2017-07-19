using Microsoft.EntityFrameworkCore;
using Pets.Core.Domain;
using Pets.Core.Repositories;
using Pets.Infrastructure.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pets.Infrastructure.Repositories
{
    public class AnimalRepository : IAnimalRepository
    {
        private readonly PetsContext _context;

        public AnimalRepository(PetsContext context)
        {
            _context = context;
        }

        public async Task<Animal> GetAsync(Guid id)
        {
            return await _context.Animals.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Animal> GetAsync(Guid userId, string name)
        {
            return await _context.Animals.Where(x => x.UserId == userId && x.Name.Equals(name)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Animal>> GetAllAsync(Guid userId)
        {
            return await Task.FromResult(_context.Animals.Where(x => x.UserId == userId).ToList());
        }

        public async Task AddAsync(Animal animal)
        {
            await _context.Animals.AddAsync(animal);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            var animal = await GetAsync(id);
            _context.Animals.Remove(animal);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Animal animal)
        {
            _context.Animals.Update(animal);

            await _context.SaveChangesAsync();
        }
    }
}
