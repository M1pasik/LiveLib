using LiveLib.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiveLib.Database.ModelsConfiguration
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(b => b.PublicatedAt)
                .IsRequired();

            builder.Property(b => b.PageCount)
                .IsRequired();

            builder.Property(b => b.Description)
                .HasMaxLength(4000);

            builder.Property(b => b.AverageRating)
                .HasPrecision(3, 2);

            builder.Property(b => b.Isbn)
                .HasMaxLength(17);

            builder.HasOne(b => b.Genre)
                .WithMany(g => g.Books)
                .HasForeignKey(b => b.GenreId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.BookPublisher)
                .WithMany(bp => bp.Books)
                .HasForeignKey(b => b.BookPublisherId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(b => b.Collections)
                .WithMany(c => c.Books)
                .UsingEntity(j => j.ToTable("BookCollections"));
        }
    }
}
