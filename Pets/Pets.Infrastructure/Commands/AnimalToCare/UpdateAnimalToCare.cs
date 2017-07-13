using System;

namespace Pets.Infrastructure.Commands.AnimalToCare
{
    public class UpdateAnimalToCare
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public bool IsTaken { get; set; }
    }
}
