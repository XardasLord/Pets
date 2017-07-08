using System;

namespace Pets.Core.Domain
{
    class User
    {
        public Guid Id { get; protected set; }
        public string Login { get; protected set; }
        public string FirstName { get; protected set; }
        public string LastName { get; protected set; }
        public string Email { get; protected set; }
        public DateTime CreatedAt { get; protected set; }

        protected User()
        {
        }

        public User(string login, string email)
        {
            Id = Guid.NewGuid();
            Login = login;
            Email = email;
            CreatedAt = DateTime.UtcNow;
        }

        public void SetLogin(string login)
        {
            Login = login;
        }

        public void SetFirstName(string firstName)
        {
            FirstName = firstName;
        }

        public void SetLastName(string lastName)
        {
            LastName = lastName;
        }

        public void SetEmail(string email)
        {
            Email = email;
        }
    }
}
