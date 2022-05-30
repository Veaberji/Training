using System;

namespace MusiciansAPP.Domain;

public class Album
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int PlayCount { get; set; }
    public Guid ArtistId { get; set; }
    public Artist Artist { get; set; }
}