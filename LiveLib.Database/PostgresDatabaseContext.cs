using LiveLib.Application.Interfaces;
using LiveLib.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LiveLib.Database
{
    public class PostgresDatabaseContext(DbContextOptions<PostgresDatabaseContext> options) : DbContext(options), IDatabaseContext
    {
        // Identity
        public DbSet<User> Users { get; set; }

        // Application
        public DbSet<Author> Authors { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<BookPublisher> BookPublishers { get; set; }

        public DbSet<Collection> Collections { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Review> Reviews { get; set; }


        protected override void OnModelCreating(ModelBuilder models)
        {
            models.ApplyConfigurationsFromAssembly(typeof(PostgresDatabaseContext).Assembly);
        }
    }
}
