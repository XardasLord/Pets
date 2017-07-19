using System;

namespace Pets.Infrastructure.Commands.AnimalToCare
{
    public class GetAnimalToCare
    {
        public Guid UserId { get; set; }
        public Guid AnimalId { get; set; }
    }
}
