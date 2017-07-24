using Pets.Core.Exceptions;
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
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new DomainException(ErrorCodes.InvalidEmail, "Email can not be empty.");
            }

            Email = email.ToLowerInvariant();
        }

        public void SetFirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new DomainException(ErrorCodes.InvalidFirstname, "Firstname can not be empty.");
            }

            FirstName = firstName;
        }

        public void SetLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new DomainException(ErrorCodes.InvalidLastname, "Lastname can not be empty.");
            }

            LastName = lastName;
        }

        public void SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new DomainException(ErrorCodes.InvalidPassword, "Password can not be empty.");
            }
            if (password.Length < 4)
            {
                throw new DomainException(ErrorCodes.InvalidPassword, "Password must contain at least 4 characters.");
            }
            if (password.Length > 100)
            {
                throw new DomainException(ErrorCodes.InvalidPassword, "Password can not contain more that 100 characters.");
            }

            Password = password;
        }
    }
}
