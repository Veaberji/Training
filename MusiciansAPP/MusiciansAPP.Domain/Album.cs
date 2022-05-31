using System;
using System.Collections.Generic;

namespace MusiciansAPP.Domain;

public class Album
{
    public Album()
    {
        Tracks = new List<Track>();
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public int PlayCount { get; set; }
    public Guid ArtistId { get; set; }
    public Artist Artist { get; set; }
    public IEnumerable<Track> Tracks { get; set; }
}