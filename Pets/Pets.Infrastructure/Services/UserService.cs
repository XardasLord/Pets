using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pets.Infrastructure.DTO;
using Pets.Core.Repositories;
using Pets.Core.Domain;
using Pets.Infrastructure.Extensions;
using Pets.Infrastructure.Exceptions;

namespace Pets.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAnimalToCareRepository _animalToCareRepository;
        private readonly IAnimalRepository _animalRepository;
        private readonly IEncrypter _encrypter;

        public UserService(IUserRepository userRepository,
            IAnimalToCareRepository animalToCareRepository,
            IAnimalRepository animalRepository,
            IEncrypter encrypter)
        {
            _userRepository = userRepository;
            _animalToCareRepository = animalToCareRepository;
            _animalRepository = animalRepository;
            _encrypter = encrypter;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var usersDto = new HashSet<UserDto>();

            foreach(var user in users)
            {
                usersDto.Add(new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    CreatedAt = user.CreatedAt
                });
            }

            return usersDto;
        }

        public async Task<UserDto> GetAsync(string email)
        {
            var user = await _userRepository.GetOrFailAsync(email);

            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreatedAt = user.CreatedAt
            };

            return userDto;
        }

        public async Task<IEnumerable<AnimalToCareDto>> GetCaringAnimalsAsync(string email)
        {
            var user = await GetAsync(email);
            var animalsToCare = await _animalToCareRepository.GetAnimalsCaringByUserAsync(user.Id);

            var animalsToCareDto = new HashSet<AnimalToCareDto>();

            foreach (var toCare in animalsToCare)
            {
                var tmpAnimal = await _animalRepository.GetAsync(toCare.AnimalId);
                var animalDto = new AnimalDto
                {
                    Id = toCare.Id,
                    Name = tmpAnimal.Name,
                    User = await GetAsync(email),
                    YearOfBirth = toCare.Animal.YearOfBirth
                };

                animalsToCareDto.Add(new AnimalToCareDto
                {
                    Id = toCare.Id,
                    Animal = animalDto,
                    DateFrom = toCare.DateFrom,
                    DateTo = toCare.DateTo,
                    IsTaken = toCare.IsTaken
                });
            }

            return animalsToCareDto;
        }

        public async Task<IEnumerable<AnimalToCareDto>> GetCaringAnimalsArchiveAsync(string email)
        {
            var user = await GetAsync(email);
            var animalsToCare = await _animalToCareRepository.GetAnimalsCaringByUserArchiveAsync(user.Id);

            var animalsToCareDto = new HashSet<AnimalToCareDto>();

            foreach (var toCare in animalsToCare)
            {
                var tmpAnimal = await _animalRepository.GetAsync(toCare.AnimalId);
                var animalDto = new AnimalDto
                {
                    Id = toCare.Id,
                    Name = tmpAnimal.Name,
                    User = await GetAsync(email),
                    YearOfBirth = toCare.Animal.YearOfBirth
                };

                animalsToCareDto.Add(new AnimalToCareDto
                {
                    Id = toCare.Id,
                    Animal = animalDto,
                    DateFrom = toCare.DateFrom,
                    DateTo = toCare.DateTo,
                    IsTaken = toCare.IsTaken
                });
            }

            return animalsToCareDto;
        }

        public async Task UpdateAsync(string email, string firstName, string lastName, string password)
        {
            var user = await _userRepository.GetOrFailAsync(email);
            
            user.SetEmail(email);
            user.SetFirstName(firstName);
            user.SetLastName(lastName);

            var hash = _encrypter.GetHash(password, user.Salt);
            user.SetPassword(hash);

            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(string email)
        {
            var user = await _userRepository.GetOrFailAsync(email);

            await _userRepository.RemoveAsync(user.Id);
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetOrFailAsync(email);

            var hash = _encrypter.GetHash(password, user.Salt);
            if (user.Password == hash)
            {
                return true;
            }

            throw new ServiceException(ErrorCodes.InvalidCredentials, "Invalid credentials");
        }

        public async Task RegisterAsync(string email, string firstName, string lastName, string password)
        {
            var user = await _userRepository.GetAsync(email);
            if(user != null)
            {
                throw new ServiceException(ErrorCodes.UserAlreadyExist, $"User with email {user.Email} already exists.");
            }
            
            var salt = _encrypter.GetSalt(password);
            var hash = _encrypter.GetHash(password, salt);
            user = new User(Guid.NewGuid(), email, firstName, lastName, hash, salt);

            await _userRepository.AddAsync(user);
        }
    }
}
