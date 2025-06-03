using LiveLib.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LiveLib.Application.Interfaces
{
    public interface IDatabaseContext
    {
        DbSet<User> Users { get; set; }

        DbSet<Author> Authors { get; set; }

        DbSet<Book> Books { get; set; }

        DbSet<BookPublisher> BookPublishers { get; set; }

        DbSet<Collection> Collections { get; set; }

        DbSet<Genre> Genres { get; set; }

        DbSet<Review> Reviews { get; set; }


        Task<int> SaveChangesAsync(CancellationToken cts);
    }
}
