using System;

namespace Pets.Core.Domain
{
    class CaringAnimal
    {
        public Guid Id { get; protected set; }
        public Guid AnimalToCareId { get; protected set; }
        public Guid UserId { get; protected set; }

        protected CaringAnimal()
        {
        }

        public CaringAnimal(AnimalToCare animalToCare, User user)
        {
            Id = Guid.NewGuid();
            AnimalToCareId = animalToCare.Id;
            UserId = user.Id;
        }
    }
}
