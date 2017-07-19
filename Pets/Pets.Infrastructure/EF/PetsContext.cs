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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<User>()
                .HasMany(x => x.Animals)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);
            modelBuilder.Entity<User>()
                .HasMany(x => x.AnimalsToCare)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<Animal>().HasKey(x => x.Id);
            modelBuilder.Entity<Animal>()
                .HasMany(x => x.AnimalsToCare)
                .WithOne(x => x.Animal)
                .HasForeignKey(x => x.AnimalId);

            modelBuilder.Entity<AnimalToCare>().HasKey(x => x.Id);


            //modelBuilder.Entity<CaringAnimal>().HasKey(x => x.Id);
            //modelBuilder.Entity<CaringAnimal>()
            //    .HasOne(x => x.User)
            //    .WithMany(x => x.CaringAnimals)
            //    .HasForeignKey(x => x.UserId);
        }
    }
}
