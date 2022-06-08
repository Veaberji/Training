using MusiciansAPP.Domain.Constraints;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusiciansAPP.Domain;

public class Album
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
    public Guid ArtistId { get; set; }
    public Artist Artist { get; set; }
    public IEnumerable<Track> Tracks { get; set; }

    public bool IsAlbumHasPlayCount()
    {
        return PlayCount is not null;
    }
}