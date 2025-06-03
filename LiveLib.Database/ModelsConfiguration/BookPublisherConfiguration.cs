using LiveLib.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiveLib.Database.ModelsConfiguration
{
    public class BookPublisherConfiguration : IEntityTypeConfiguration<BookPublisher>
    {
        public void Configure(EntityTypeBuilder<BookPublisher> builder)
        {
            builder.HasKey(bp => bp.Id);

            builder.Property(bp => bp.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(bp => bp.Description)
                .HasMaxLength(2000);

            builder.HasMany(bp => bp.Books)
                .WithOne(b => b.BookPublisher)
                .HasForeignKey(b => b.BookPublisherId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
