﻿using Pets.Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pets.Core.Repositories
{
    public interface IAnimalToCareRepository
    {
        Task<IEnumerable<AnimalToCare>> GetAsync(Guid animalId);
        Task<IEnumerable<AnimalToCare>> GetAllActiveAsync();
        Task<IEnumerable<AnimalToCare>> GetAllArchiveAsync();
        Task<IEnumerable<AnimalToCare>> GetAnimalsCaringByUserAsync(Guid userId);
        Task<IEnumerable<AnimalToCare>> GetAnimalsCaringByUserArchiveAsync(Guid userId);

        Task AddAsync(AnimalToCare animalToCare);
        Task UpdateAsync(AnimalToCare animalToCare);
        Task RemoveAsync(AnimalToCare animalToCare);
    }
}
