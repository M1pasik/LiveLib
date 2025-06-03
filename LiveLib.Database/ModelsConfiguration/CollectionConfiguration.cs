using LiveLib.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiveLib.Database.ModelsConfiguration
{
    public class CollectionConfiguration : IEntityTypeConfiguration<Collection>
    {
        public void Configure(EntityTypeBuilder<Collection> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasOne(c => c.OwnerUser)
                .WithMany(u => u.Collections)
                .HasForeignKey(c => c.OwnerUserId);

            builder.HasMany(c => c.UsersSubscribers)
                .WithMany(u => u.SubscribedCollections)
                .UsingEntity(j => j.ToTable("CollectionSubscribers"));

            builder.HasMany(c => c.Books)
                .WithMany(u => u.Collections)
                .UsingEntity(j => j.ToTable("BookCollections"));
        }
    }
}
