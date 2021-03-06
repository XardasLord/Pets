﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pets.Infrastructure.DTO;
using Pets.Core.Repositories;
using System.Linq;
using Pets.Core.Domain;
using Pets.Infrastructure.Extensions;
using Pets.Infrastructure.Exceptions;

namespace Pets.Infrastructure.Services
{
    public class AnimalToCareService : IAnimalToCareService
    {
        IAnimalToCareRepository _animalToCareRepository;
        IAnimalRepository _animalRepository;
        IUserRepository _userRepository;
        IAnimalService _animalService;

        public AnimalToCareService(IAnimalToCareRepository animalToCareRepository,
            IAnimalRepository animalRepository,
            IUserRepository userRepository,
            IAnimalService animalService)
        {
            _animalToCareRepository = animalToCareRepository;
            _animalRepository = animalRepository;
            _userRepository = userRepository;
            _animalService = animalService;
        }

        public async Task<IEnumerable<AnimalToCareDto>> GetAllActiveAsync()
        {
            var animalsToCare = await _animalToCareRepository.GetAllActiveAsync();
            var animals = new HashSet<AnimalToCareDto>();

            foreach(var animalToCare in animalsToCare)
            {
                var animalDto = await _animalService.GetAsync(animalToCare.AnimalId);

                animals.Add(new AnimalToCareDto
                {

                    Id = animalToCare.Id,
                    Animal = animalDto,
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
                var animalDto = await _animalService.GetAsync(animalToCare.AnimalId);

                animals.Add(new AnimalToCareDto
                {
                    Id = animalToCare.Id,
                    Animal = animalDto,
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
                var animalDto = await _animalService.GetAsync(animalId);

                animals.Add(new AnimalToCareDto
                {
                    Id = animalToCare.Id,
                    Animal = animalDto,
                    DateFrom = animalToCare.DateFrom,
                    DateTo = animalToCare.DateTo,
                    IsTaken = animalToCare.IsTaken
                });
            }

            return animals;
        }

        public async Task AddToCareListAsync(Guid animalId, DateTime dateFrom, DateTime dateTo)
        {
            var animal = await _animalRepository.GetOrFailAsync(animalId);

            var animalToCare = new AnimalToCare(animal.Id, dateFrom, dateTo);

            await _animalToCareRepository.AddAsync(animalToCare);
        }

        public async Task GetAnimalToCareAsync(Guid animalId, Guid userId)
        {
            var animalToGet = await GetActiveAnimalToCareById(animalId);
            if (animalToGet == null)
            {
                throw new ServiceException(ErrorCodes.AnimalNotAvailable, "Animal with given ID is not available to get, because there is no active animal with that ID.");
            }

            var user = await _userRepository.GetOrFailAsync(userId);
            animalToGet.SetUserId(user.Id);
            animalToGet.SetIsTaken(true);

            await _animalToCareRepository.UpdateAsync(animalToGet);
        }

        public async Task DeleteAsync(Guid animalId)
        {
            var animalToDelete = await GetActiveAnimalToCareById(animalId);
            if(animalToDelete == null)
            {
                throw new ServiceException(ErrorCodes.AnimalNotAvailable, "Animal with given ID is not available to delete, because there is no active animal with that ID.");
            }

            await _animalToCareRepository.RemoveAsync(animalToDelete);
        }

        public async Task UpdateAsync(Guid animalId, DateTime dateFrom, DateTime dateTo, bool isTaken)
        {
            var animalToCareToUpdate = await GetActiveAnimalToCareById(animalId);
            if (animalToCareToUpdate == null)
            {
                throw new ServiceException(ErrorCodes.AnimalNotAvailable, "Animal with given ID is not available to update, because there is no active animal with that ID.");
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
