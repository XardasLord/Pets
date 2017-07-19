using Pets.Infrastructure.Commands.Animals;
using Pets.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pets.Infrastructure.Services
{
    public interface IAnimalToCareService
    {
        Task<IEnumerable<AnimalToCareDto>> GetAsync(Guid animalId);
        Task<IEnumerable<AnimalToCareDto>> GetAllActiveAsync();
        Task<IEnumerable<AnimalToCareDto>> GetAllArchiveAsync();

        Task AddToCareListAsync(Guid animalId, DateTime dateFrom, DateTime dateTo);
        Task GetAnimalToCareAsync(Guid animalId, Guid userId);
        Task UpdateAsync(Guid animalId, DateTime dateFrom, DateTime dateTo, bool isTaken);
        Task DeleteAsync(Guid animalId);
    }
}
