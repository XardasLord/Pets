using System;

namespace Pets.Infrastructure.DTO
{
    public class AnimalDto
    {
        public Guid Id { get; set; }
        public UserDto User { get; set; }
        public string Name { get; set; }
        public int YearOfBirth { get; set; }
    }
}
