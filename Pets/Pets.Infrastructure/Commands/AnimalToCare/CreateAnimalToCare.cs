using System;

namespace Pets.Infrastructure.Commands.AnimalToCare
{
    public class CreateAnimalToCare
    {
        public Guid AnimalId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
