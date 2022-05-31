using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusiciansAPP.DAL.DBDataProvider.Constraints;
using MusiciansAPP.Domain;

namespace MusiciansAPP.DAL.DBDataProvider.EntityConfigurations
{
    public class TrackConfiguration : IEntityTypeConfiguration<Track>
    {
        public void Configure(EntityTypeBuilder<Track> builder)
        {
            builder
                .Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(TrackConstraints.NameMaxLength);

            builder
                .Property(a => a.PlayCount)
                .IsRequired()
                .HasDefaultValue(TrackConstraints.PlayCountMinValue);

            builder
                .Property(a => a.DurationInSeconds)
                .IsRequired(false);

            builder
                .Property(a => a.AlbumId)
                .IsRequired(false);
        }
    }
}
