using MusiciansAPP.Domain.Constraints;
using System;
using System.ComponentModel.DataAnnotations;

namespace MusiciansAPP.Domain;

public class Track
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(TrackConstraints.NameMaxLength)]
    public string Name { get; set; }

    [Range(TrackConstraints.PlayCountMinValue, int.MaxValue)]
    public int? PlayCount { get; set; }

    [Range(TrackConstraints.DurationInSecondsMinValue, int.MaxValue)]
    public int? DurationInSeconds { get; set; }
    public Guid ArtistId { get; set; }
    public Artist Artist { get; set; }
    public Guid? AlbumId { get; set; }
    public Album Album { get; set; }

    public bool IsTrackHasPlayCount()
    {
        return PlayCount is not null;
    }

    public bool IsTrackDurationInSeconds()
    {
        return DurationInSeconds is not null;
    }
}