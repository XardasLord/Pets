using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Pets.Infrastructure.Commands.Animals;
using Pets.Infrastructure.DTO;
using Pets.Core.Repositories;
using System.Linq;
using Pets.Core.Domain;

namespace Pets.Infrastructure.Services
{
    public class AnimalToCareService : IAnimalToCareService
    {
        IAnimalToCareRepository _animalToCareRepository;
        IAnimalRepository _animalRepository;

        public AnimalToCareService(IAnimalToCareRepository animalToCareRepository,
            IAnimalRepository animalRepository)
        {
            _animalToCareRepository = animalToCareRepository;
            _animalRepository = animalRepository;
        }

        public async Task<IEnumerable<AnimalToCareDto>> GetAllActiveAsync()
        {
            var animalsToCare = await _animalToCareRepository.GetAllActiveAsync();
            var animals = new HashSet<AnimalToCareDto>();

            foreach(var animalToCare in animalsToCare)
            {
                animals.Add(new AnimalToCareDto
                {
                    Id = animalToCare.Id,
                    Animal = new AnimalDto
                    {
                        Id = animalToCare.Animal.Id,
                        Name = animalToCare.Animal.Name,
                        UserId = animalToCare.Animal.UserId,
                        YearOfBirth = animalToCare.Animal.YearOfBirth
                    },
                    DateFrom = animalToCare.DateFrom,
                    DateTo = animalToCare.DateTo,
                    IsTaken = animalToCare.IsTaken
                });
            }

            return animals;
        }

        public async Task<IEnumerable<AnimalToCareDto>> GetAllArchiveAsync()
        {
            var animalsToCare = await _animalToCareRepository.GetAllArchiveAsync();
            var animals = new HashSet<AnimalToCareDto>();

            foreach (var animalToCare in animalsToCare)
            {
                animals.Add(new AnimalToCareDto
                {
                    Id = animalToCare.Id,
                    Animal = new AnimalDto
                    {
                        Id = animalToCare.Animal.Id,
                        Name = animalToCare.Animal.Name,
                        UserId = animalToCare.Animal.UserId,
                        YearOfBirth = animalToCare.Animal.YearOfBirth
                    },
                    DateFrom = animalToCare.DateFrom,
                    DateTo = animalToCare.DateTo,
                    IsTaken = animalToCare.IsTaken
                });
            }

            return animals;
        }

        public async Task<IEnumerable<AnimalToCareDto>> GetAsync(Guid animalId)
        {
            var animalsToCare = await _animalToCareRepository.GetAsync(animalId);
            var animals = new HashSet<AnimalToCareDto>();

            foreach (var animalToCare in animalsToCare)
            {
                animals.Add(new AnimalToCareDto
                {
                    Id = animalToCare.Id,
                    Animal = new AnimalDto
                    {
                        Id = animalToCare.Animal.Id,
                        Name = animalToCare.Animal.Name,
                        UserId = animalToCare.Animal.UserId,
                        YearOfBirth = animalToCare.Animal.YearOfBirth
                    },
                    DateFrom = animalToCare.DateFrom,
                    DateTo = animalToCare.DateTo,
                    IsTaken = animalToCare.IsTaken
                });
            }

            return animals;
        }

        public async Task AddAsync(Guid animalId, DateTime dateFrom, DateTime dateTo)
        {
            var animal = await _animalRepository.GetAsync(animalId);
            if (animal == null)
            {
                throw new Exception("Animal of given ID doesn't exist.");
            }

            var animalToCare = new AnimalToCare(animal, dateFrom, dateTo);

            await _animalToCareRepository.AddAsync(animalToCare);
        }

        public async Task DeleteAsync(Guid animalId)
        {
            // Check if the user who wants to delete information is the owner of that animal.

            var animalToDelete = await GetActiveAnimalToCareById(animalId);
            if(animalToDelete == null)
            {
                throw new Exception("Animal with given ID is not available to delete, because there is no active animal.");
            }

            await _animalToCareRepository.RemoveAsync(animalToDelete);
        }

        public async Task UpdateAsync(Guid animalId, DateTime dateFrom, DateTime dateTo, bool isTaken)
        {
            var animalToCareToUpdate = await GetActiveAnimalToCareById(animalId);
            if (animalToCareToUpdate == null)
            {
                throw new Exception("Animal with given ID is not available to delete, because there is no active animal.");
            }

            animalToCareToUpdate.SetIsTaken(isTaken);
            animalToCareToUpdate.SetDateFrom(dateFrom);
            animalToCareToUpdate.SetDateTo(dateTo);

            await _animalToCareRepository.UpdateAsync(animalToCareToUpdate);
        }

        private async Task<AnimalToCare> GetActiveAnimalToCareById(Guid animalId)
        {
            var animals = await _animalToCareRepository.GetAsync(animalId);
            var activeAnimal = animals.Where(x => x.IsTaken == false && x.DateFrom >= DateTime.UtcNow).FirstOrDefault();

            return activeAnimal;
        }
    }
}
