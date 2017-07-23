using Pets.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pets.Infrastructure.Services
{
    public interface IAnimalService
    {
        Task<AnimalDto> GetAsync(Guid animalId);
        Task<AnimalDto> GetAsync(string email, string name);
        Task<IEnumerable<AnimalDto>> GetAllAsync(string email);
        Task AddAsync(string email, string name, int yearOfBirth);
        Task UpdateAsync(string email, string oldName, string newName, int yearOfBirth);
        Task DeleteAsync(string email, string name);
    }
}
