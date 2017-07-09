﻿using Pets.Core.Domain;
using Pets.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pets.Infrastructure.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        ISet<User> _users = new HashSet<User>()
        {
            new User(Guid.NewGuid(), "user1@email.pl", "Paweł", "Kowalewicz", "secret123", "salt"),
            new User(Guid.NewGuid(), "user2@email.pl", "Second", "User", "secret123", "salt")
        };

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
        }

        public async Task UpdateAsync(User user)
        {
            var userToUpdate = await GetAsync(user.Id);
            await RemoveAsync(user.Id);

            _users.Add(user);
        }

        public async Task RemoveAsync(Guid id)
        {
            var userToRemove = await GetAsync(id);
            _users.Remove(userToRemove);

            await Task.CompletedTask;
        }
    }
}
