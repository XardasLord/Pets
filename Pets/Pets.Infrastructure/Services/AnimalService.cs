using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pets.Infrastructure.DTO;
using Pets.Core.Repositories;
using Pets.Core.Domain;

namespace Pets.Infrastructure.Services
{
    public class AnimalService : IAnimalService
    {
        private IAnimalRepository _animalRepository;

        public AnimalService(IAnimalRepository animalRepository)
        {
            _animalRepository = animalRepository;
        }


        public async Task AddAsync(Guid userId, string name, int yearOfBirth)
        {
            var animal = await _animalRepository.GetAsync(userId, name);
            if (animal != null)
            {
                throw new Exception($"Animal with name {name} already exists for this user.");
            }

            var newAnimal = new Animal(userId, name);
            newAnimal.SetYearOfBirth(yearOfBirth);

            await _animalRepository.AddAsync(newAnimal);
        }

        public async Task DeleteAsync(Guid userId, string name)
        {
            var animal = await _animalRepository.GetAsync(userId, name);
            if (animal == null)
            {
                throw new Exception($"Animal with name {name} does not exist for this user.");
            }

            await _animalRepository.RemoveAsync(animal.Id);
        }

        public async Task<IEnumerable<AnimalDto>> GetAllAsync(Guid userId)
        {
            var animals = await _animalRepository.GetAllAsync(userId);
            var animalsDto = new HashSet<AnimalDto>();

            foreach(var animal in animals)
            {
                animalsDto.Add(new AnimalDto
                {
                    Id = animal.Id,
                    UserId = animal.UserId,
                    Name = animal.Name,
                    YearOfBirth = animal.YearOfBirth
                });
            }

            return animalsDto;
        }

        public async Task<AnimalDto> GetAsync(Guid userId, string name)
        {
            var animal = await _animalRepository.GetAsync(userId, name);
            if (animal == null)
            {
                throw new Exception($"Animal with name {name} does not exist for this user.");
            }

            return new AnimalDto
            {
                Id = animal.Id,
                UserId = animal.UserId,
                Name = animal.Name,
                YearOfBirth = animal.YearOfBirth
            };
        }

        public async Task UpdateAsync(Guid userId, string name, int yearOfBirth)
        {
            var animal = await _animalRepository.GetAsync(userId, name);
            if (animal == null)
            {
                throw new Exception($"Animal with name {name} does not exist for this user.");
            }

            animal.SetName(name);
            animal.SetYearOfBirth(yearOfBirth);

            await _animalRepository.UpdateAsync(animal);
        }
    }
}
