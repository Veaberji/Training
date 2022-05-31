using MusiciansAPP.Domain.Constraints;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusiciansAPP.Domain;

public class Artist
{
    public Artist()
    {
        Tracks = new List<Track>();
        Albums = new List<Album>();
    }

    public Guid Id { get; set; }

    [Required]
    [MaxLength(ArtistConstraints.NameMaxLength)]
    public string Name { get; set; }

    [Required]
    [MaxLength(ArtistConstraints.ImageUrlMaxLength)]
    public string ImageUrl { get; set; }

    [MaxLength(ArtistConstraints.BiographyMaxLength)]
    public string Biography { get; set; }
    public IEnumerable<Track> Tracks { get; set; }
    public IEnumerable<Album> Albums { get; set; }
}