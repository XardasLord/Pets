using System;

namespace Pets.Core.Domain
{
    public class Animal
    {
        public Guid Id { get; protected set; }
        public Guid UserId { get; protected set; }
        public string Name { get; protected set; }
        public int YearOfBirth { get; protected set; }

        protected Animal()
        {
        }

        public Animal(Guid userId, string name)
        {
            UserId = userId;
            Name = name;
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public void SetYearOfBirth(int year)
        {
            YearOfBirth = year;
        }
    }
}
