using System;

namespace MusiciansAPP.Domain;

public class Track
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int PlayCount { get; set; }
    public int? DurationInSeconds { get; set; }
    public Guid ArtistId { get; set; }
    public Artist Artist { get; set; }
    public Guid? AlbumId { get; set; }
    public Album Album { get; set; }
}