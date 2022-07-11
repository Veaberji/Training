using System.Collections.Generic;

namespace MusiciansAPP.DAL.DALModels;

public class AlbumDAL
{
    public AlbumDAL()
    {
        Tracks = new List<TrackDAL>();
    }

    public string Name { get; set; }

    public string ImageUrl { get; set; }

    public int PlayCount { get; set; }

    public string ArtistName { get; set; }

    public IEnumerable<TrackDAL> Tracks { get; set; }
}