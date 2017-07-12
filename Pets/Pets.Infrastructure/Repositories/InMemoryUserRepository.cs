using Microsoft.EntityFrameworkCore;
using Pets.Core.Domain;
using Pets.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pets.Infrastructure.Repositories
{
    public class InMemoryUserRepository : DbContext, IUserRepository
    {
        DbSet<User> _users { get; set; }

        public InMemoryUserRepository(DbContextOptions<InMemoryUserRepository> options)
            : base(options)
        {
        }

        public async Task<User> GetAsync(Guid id)
        {
            return await Task.FromResult(_users.SingleOrDefault(x => x.Id == id));
        }

        public async Task<User> GetAsync(string email)
        {
            return await Task.FromResult(_users.FirstOrDefault(x => x.Email == email));
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await Task.FromResult(_users);
        }

        public async Task AddAsync(User user)
        {
            await Task.FromResult(_users.Add(user));
            await SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _users.Update(user);
            await SaveChangesAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            var userToRemove = await GetAsync(id);
            _users.Remove(userToRemove);

            await SaveChangesAsync();
        }
    }
}
