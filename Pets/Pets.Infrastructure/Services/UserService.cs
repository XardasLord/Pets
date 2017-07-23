using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pets.Infrastructure.DTO;
using Pets.Core.Repositories;
using Pets.Core.Domain;

namespace Pets.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAnimalToCareRepository _animalToCareRepository;
        private readonly IEncrypter _encrypter;

        public UserService(IUserRepository userRepository,
            IAnimalToCareRepository animalToCareRepository, 
            IEncrypter encrypter)
        {
            _userRepository = userRepository;
            _animalToCareRepository = animalToCareRepository;
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
            var user = await _userRepository.GetAsync(email);
            if(user == null)
            {
                throw new Exception($"User with email: {email} doesn't exist.");
            }

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

            foreach (var animal in animalsToCare)
            {
                animalsToCareDto.Add(new AnimalToCareDto
                {
                    Id = animal.Id,
                    DateFrom = animal.DateFrom,
                    DateTo = animal.DateTo,
                    IsTaken = animal.IsTaken
                });
            }

            return animalsToCareDto;
        }

        public async Task<IEnumerable<AnimalToCareDto>> GetCaringAnimalsArchiveAsync(string email)
        {
            var user = await GetAsync(email);
            var animalsToCare = await _animalToCareRepository.GetAnimalsCaringByUserArchiveAsync(user.Id);

            var animalsToCareDto = new HashSet<AnimalToCareDto>();

            foreach (var animal in animalsToCare)
            {
                animalsToCareDto.Add(new AnimalToCareDto
                {
                    Id = animal.Id,
                    DateFrom = animal.DateFrom,
                    DateTo = animal.DateTo,
                    IsTaken = animal.IsTaken
                });
            }

            return animalsToCareDto;
        }

        public async Task UpdateAsync(string email, string firstName, string lastName, string password)
        {
            var user = await _userRepository.GetAsync(email);
            if(user == null)
            {
                throw new Exception($"User with email: {email} doesn't exist.");
            }

            user.SetEmail(email);
            user.SetFirstName(firstName);
            user.SetLastName(lastName);
            user.SetPassword(password);

            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(string email)
        {
            var user = await _userRepository.GetAsync(email);
            if (user == null)
            {
                throw new Exception($"User with email: {email} doesn't exist.");
            }

            await _userRepository.RemoveAsync(user.Id);
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetAsync(email);
            if (user == null)
            {
                throw new Exception("Invalid credentials.");
            }

            var hash = _encrypter.GetHash(password, user.Salt);
            if (user.Password == hash)
            {
                return true;
            }

            throw new Exception("Invalid credentials");
        }

        public async Task RegisterAsync(string email, string firstName, string lastName, string password)
        {
            var user = await _userRepository.GetAsync(email);
            if(user != null)
            {
                throw new Exception($"User with email: {user.Email} already exists.");
            }
            
            var salt = _encrypter.GetSalt(password);
            var hash = _encrypter.GetHash(password, salt);
            user = new User(Guid.NewGuid(), email, firstName, lastName, hash, salt);

            await _userRepository.AddAsync(user);
        }
    }
}
