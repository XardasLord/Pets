using Pets.Infrastructure.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pets.Infrastructure.Services
{
    public interface IUserService
    {
        Task<UserDto> GetAsync(string email);
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<IEnumerable<AnimalToCareDto>> GetCaringAnimalsAsync(string email);
        Task<IEnumerable<AnimalToCareDto>> GetCaringAnimalsArchiveAsync(string email);
        Task UpdateAsync(string email, string firstName, string lastName, string password);
        Task DeleteAsync(string email);
        Task RegisterAsync(string email, string firstName, string lastName, string password);
        Task<bool> LoginAsync(string email, string password);
    }
}
