using MusiciansAPP.Domain.Constraints;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MusiciansAPP.Domain;

public class Album : Entity
{
    public Album()
    {
        Tracks = new List<Track>();
    }

    public Guid Id { get; set; }

    [Required]
    [MaxLength(AlbumConstraints.NameMaxLength)]
    public string Name { get; set; }

    [Required]
    [MaxLength(AlbumConstraints.ImageUrlMaxLength)]
    public string ImageUrl { get; set; }

    [Range(AlbumConstraints.PlayCountMinValue, int.MaxValue)]
    public int? PlayCount { get; set; }
    public Artist Artist { get; set; }
    public IEnumerable<Track> Tracks { get; set; }

    public bool IsAlbumHasPlayCount()
    {
        return PlayCount is not null;
    }

    public bool IsAlbumHasImageUrl()
    {
        return !string.IsNullOrWhiteSpace(ImageUrl);
    }

    public bool IsAlbumTracksDetailsUpToDate()
    {
        return Tracks.Any() && Tracks.All(track => track.IsTrackHasDurationInSeconds());
    }

    protected override bool IsFull()
    {
        return IsAlbumHasImageUrl() && IsAlbumHasPlayCount();
    }
}