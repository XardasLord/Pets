using Pets.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pets.Infrastructure.Services
{
    public interface IUserService
    {
        Task<UserDto> GetAsync(string email);
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task RegisterAsync(string email, string firstName, string lastName, string password);
        Task LoginAsync(string email, string password);
    }
}
