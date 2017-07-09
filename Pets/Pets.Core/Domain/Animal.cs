using System;

namespace Pets.Core.Domain
{
    public abstract class Animal
    {
        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
        public int YearOfBirth { get; protected set; }

        protected Animal()
        {
        }

        public Animal(string name)
        {
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
