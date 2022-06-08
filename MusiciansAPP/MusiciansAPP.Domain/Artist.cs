﻿using MusiciansAPP.Domain.Constraints;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusiciansAPP.Domain;

public class Artist
{
    public Artist()
    {
        Tracks = new List<Track>();
        Albums = new List<Album>();
        SimilarArtists = new List<Artist>();
        ReverseSimilarArtists = new List<Artist>();
    }

    public Guid Id { get; set; }

    [Required]
    [MaxLength(ArtistConstraints.NameMaxLength)]
    public string Name { get; set; }

    [MaxLength(ArtistConstraints.ImageUrlMaxLength)]
    public string ImageUrl { get; set; }

    [Column(TypeName = "ntext")]
    [MaxLength(ArtistConstraints.BiographyMaxLength)]
    public string Biography { get; set; }
    public IEnumerable<Track> Tracks { get; set; }
    public IEnumerable<Album> Albums { get; set; }

    [InverseProperty(nameof(ReverseSimilarArtists))]
    public List<Artist> SimilarArtists { get; set; }

    [InverseProperty(nameof(SimilarArtists))]
    public IEnumerable<Artist> ReverseSimilarArtists { get; set; }

    public bool IsArtistDetailsUpToDate()
    {
        return IsArtistHasImageUrl() && Biography is not null;
    }

    public bool IsArtistHasImageUrl()
    {
        return ImageUrl is not null;
    }
}