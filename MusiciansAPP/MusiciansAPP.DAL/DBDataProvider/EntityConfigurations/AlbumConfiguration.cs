using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusiciansAPP.DAL.DBDataProvider.Constraints;
using MusiciansAPP.Domain;

namespace MusiciansAPP.DAL.DBDataProvider.EntityConfigurations
{
    public class AlbumConfiguration : IEntityTypeConfiguration<Album>
    {
        public void Configure(EntityTypeBuilder<Album> builder)
        {
            builder
                .Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(AlbumConstraints.NameMaxLength);

            builder
                .Property(a => a.ImageUrl)
                .IsRequired()
                .HasMaxLength(AlbumConstraints.ImageUrlMaxLength);

            builder
                .Property(a => a.PlayCount)
                .IsRequired()
                .HasDefaultValue(AlbumConstraints.PlayCountMinValue);
        }
    }
}
