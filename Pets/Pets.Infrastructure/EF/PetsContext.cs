using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Pets.Core.Domain;
using Pets.Infrastructure.Settings;

namespace Pets.Infrastructure.EF
{
    public class PetsContext : DbContext
    {
        private readonly MyOptions _options;

        public DbSet<User> Users { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<AnimalToCare> AnimalsToCare { get; set; }

        public PetsContext(DbContextOptions<PetsContext> options, IOptions<MyOptions> optionsAccessor)
            : base(options)
        {
            _options = optionsAccessor.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_options.InMemoryDatabase)
            {
                optionsBuilder.UseInMemoryDatabase();

                return;
            }
            //"Server=XARDASLORD\\SQLEXPRESS;Database=Pets;Integrated Security=SSPI;"
            optionsBuilder.UseSqlServer(_options.SqlConnectionString);
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
        }
    }
}
