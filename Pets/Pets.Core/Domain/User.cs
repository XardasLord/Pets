using System;
using System.Collections.Generic;

namespace Pets.Core.Domain
{
    public class User
    {
        public Guid Id { get; protected set; }
        public string Email { get; protected set; }
        public string Password { get; protected set; }
        public string Salt { get; protected set; }
        public string FirstName { get; protected set; }
        public string LastName { get; protected set; }
        public DateTime CreatedAt { get; protected set; }

        public IEnumerable<Animal> Animals { get; protected set; }
        public IEnumerable<AnimalToCare> AnimalsToCare { get; protected set; }

        protected User()
        {
        }

        public User(Guid id, string email, string firstName, string lastName, string password, string salt)
        {
            Id = id;
            Email = email;
            Password = password;
            Salt = salt;
            FirstName = firstName;
            LastName = lastName;
            CreatedAt = DateTime.UtcNow;
        }

        public void SetEmail(string email)
        {
            Email = email;
        }

        public void SetFirstName(string firstName)
        {
            FirstName = firstName;
        }

        public void SetLastName(string lastName)
        {
            LastName = lastName;
        }

        public void SetPassword(string password)
        {
            Password = password;
        }
    }
}
