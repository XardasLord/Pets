using Microsoft.EntityFrameworkCore;
using Pets.Core.Domain;

namespace Pets.Infrastructure.EF
{
    public class PetsContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<AnimalToCare> AnimalsToCare { get; set; }

        public PetsContext(DbContextOptions<PetsContext> options)
            : base(options)
        {
        }
    }
}
