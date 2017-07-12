using Pets.Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pets.Core.Repositories
{
    public interface IAnimalRepository
    {
        Task<Animal> GetAsync(Guid id);
        Task<Animal> GetAsync(Guid userId, string name);
        Task<IEnumerable<Animal>> GetAllAsync(Guid userId);

        Task AddAsync(Animal animal);
        Task UpdateAsync(Animal animal);
        Task RemoveAsync(Guid id);
    }
}
