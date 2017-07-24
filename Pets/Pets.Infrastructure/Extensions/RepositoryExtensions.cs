using Pets.Core.Domain;
using Pets.Core.Repositories;
using Pets.Infrastructure.Exceptions;
using System;
using System.Threading.Tasks;

namespace Pets.Infrastructure.Extensions
{
    public static class RepositoryExtensions
    {
        public static async Task<User> GetOrFailAsync(this IUserRepository repository, string email)
        {
            var user = await repository.GetAsync(email);
            if (user == null)
            {
                throw new ServiceException(ErrorCodes.UserNotFound, $"User with email {email} does not exist.");
            }

            return user;
        }

        public static async Task<User> GetOrFailAsync(this IUserRepository repository, Guid userId)
        {
            var user = await repository.GetAsync(userId);
            if (user == null)
            {
                throw new ServiceException(ErrorCodes.UserNotFound, $"User with ID {userId} does not exist.");
            }

            return user;
        }

        public static async Task<Animal> GetOrFailAsync(this IAnimalRepository repository, Guid animalId)
        {
            var animal = await repository.GetAsync(animalId);
            if (animal == null)
            {
                throw new ServiceException(ErrorCodes.AnimalNotFound, $"Animal with ID {animalId} does not exist.");
            }

            return animal;
        }

        public static async Task<Animal> GetOrFailAsync(this IAnimalRepository repository, Guid userId, string name)
        {
            var animal = await repository.GetAsync(userId, name);
            if (animal == null)
            {
                throw new ServiceException(ErrorCodes.AnimalNotFound, $"Animal with name {name} does not exist for this user.");
            }

            return animal;
        }
    }
}
