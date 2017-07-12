using Pets.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pets.Infrastructure.Services
{
    public interface IAnimalService
    {
        Task<AnimalDto> GetAsync(Guid userId, string name);
        Task<IEnumerable<AnimalDto>> GetAllAsync(Guid userId);
        Task AddAsync(Guid userId, string name, int yearOfBirth);
        Task UpdateAsync(Guid userId, string name, int yearOfBirth);
        Task DeleteAsync(Guid userId, string name);
    }
}
