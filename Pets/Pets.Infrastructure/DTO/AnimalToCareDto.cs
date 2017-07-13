using System;

namespace Pets.Infrastructure.DTO
{
    public class AnimalToCareDto
    {
        public Guid Id { get; set; }
        public AnimalDto Animal { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public bool IsTaken { get; set; }
    }
}
