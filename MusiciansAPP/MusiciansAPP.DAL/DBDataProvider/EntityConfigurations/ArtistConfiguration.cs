using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusiciansAPP.DAL.DBDataProvider.Constraints;
using MusiciansAPP.Domain;

namespace MusiciansAPP.DAL.DBDataProvider.EntityConfigurations
{
    public class ArtistConfiguration : IEntityTypeConfiguration<Artist>
    {
        public void Configure(EntityTypeBuilder<Artist> builder)
        {
            builder
                .Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(ArtistConstraints.NameMaxLength);

            builder
                .Property(a => a.ImageUrl)
                .IsRequired()
                .HasMaxLength(ArtistConstraints.ImageUrlMaxLength);

            builder
                .Property(a => a.Biography)
                .IsRequired(false)
                .HasMaxLength(ArtistConstraints.BiographyMaxLength);
        }
    }
}
