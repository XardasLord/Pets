using Microsoft.EntityFrameworkCore;
using Pets.Core.Domain;
using Pets.Core.Repositories;
using Pets.Infrastructure.EF;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pets.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PetsContext _context;

        public UserRepository(PetsContext context)
        {
            _context = context;
        }

        public async Task<User> GetAsync(Guid id)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> GetAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await Task.FromResult(_context.Users);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            var userToRemove = await GetAsync(id);
            _context.Users.Remove(userToRemove);

            await _context.SaveChangesAsync();
        }
    }
}
