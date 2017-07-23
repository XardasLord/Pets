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
        private readonly IAnimalRepository _animalRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;

        public AnimalService(IAnimalRepository animalRepository,
            IUserRepository userRepository,
            IUserService userService)
        {
            _animalRepository = animalRepository;
            _userRepository = userRepository;
            _userService = userService;
        }
        
        public async Task<AnimalDto> GetAsync(Guid animalId)
        {
            var animal = await _animalRepository.GetAsync(animalId);
            if (animal == null)
            {
                throw new Exception($"Animal with id {animalId} does not exist.");
            }

            var user = await _userRepository.GetAsync(animal.UserId);
            var userDto = await _userService.GetAsync(user.Email);

            return new AnimalDto
            {
                Id = animal.Id,
                User = userDto,
                Name = animal.Name,
                YearOfBirth = animal.YearOfBirth
            };
        }

        public async Task<AnimalDto> GetAsync(string email, string name)
        {
            var user = await _userRepository.GetAsync(email);
            if (user == null)
            {
                throw new Exception($"User with email {email} does not exist.");
            }

            var animal = await _animalRepository.GetAsync(user.Id, name);
            if (animal == null)
            {
                throw new Exception($"Animal with name {name} does not exist for this user.");
            }

            var userDto = await _userService.GetAsync(email);

            return new AnimalDto
            {
                Id = animal.Id,
                User = userDto,
                Name = animal.Name,
                YearOfBirth = animal.YearOfBirth
            };
        }

        public async Task<IEnumerable<AnimalDto>> GetAllAsync(string email)
        {
            var user = await _userRepository.GetAsync(email);
            if (user == null)
            {
                throw new Exception($"User with email {email} does not exist.");
            }

            var animals = await _animalRepository.GetAllAsync(user.Id);
            var animalsDto = new HashSet<AnimalDto>();

            foreach (var animal in animals)
            {
                var userDto = await _userService.GetAsync(user.Email);

                animalsDto.Add(new AnimalDto
                {
                    Id = animal.Id,
                    User = userDto,
                    Name = animal.Name,
                    YearOfBirth = animal.YearOfBirth
                });
            }

            return animalsDto;
        }

        public async Task AddAsync(string email, string name, int yearOfBirth)
        {
            var user = await _userRepository.GetAsync(email);
            if (user == null)
            {
                throw new Exception($"User with email {email} does not exist.");
            }

            var animal = await _animalRepository.GetAsync(user.Id, name);
            if (animal != null)
            {
                throw new Exception($"Animal with name {name} already exists for this user.");
            }

            var newAnimal = new Animal(user.Id, name);
            newAnimal.SetYearOfBirth(yearOfBirth);

            await _animalRepository.AddAsync(newAnimal);
        }

        public async Task DeleteAsync(string email, string name)
        {
            var user = await _userRepository.GetAsync(email);
            if (user == null)
            {
                throw new Exception($"User with email {email} does not exist.");
            }

            var animal = await _animalRepository.GetAsync(user.Id, name);
            if (animal == null)
            {
                throw new Exception($"Animal with name {name} does not exist for this user.");
            }

            await _animalRepository.RemoveAsync(animal.Id);
        }

        public async Task UpdateAsync(string email, string oldName, string newName, int yearOfBirth)
        {
            var user = await _userRepository.GetAsync(email);
            if (user == null)
            {
                throw new Exception($"User with email {email} does not exist.");
            }

            var animal = await _animalRepository.GetAsync(user.Id, oldName);
            if (animal == null)
            {
                throw new Exception($"Animal with name {oldName} does not exist for this user.");
            }

            animal.SetName(newName);
            animal.SetYearOfBirth(yearOfBirth);

            await _animalRepository.UpdateAsync(animal);
        }
    }
}
