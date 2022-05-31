using System.Collections.Generic;

namespace MusiciansAPP.DAL.DALModels;

public class AlbumDetailsDAL
{
    public AlbumDetailsDAL()
    {
        Tracks = new List<AlbumTrackDAL>();
    }

    public string Name { get; set; }
    public string ArtistName { get; set; }
    public string ImageUrl { get; set; }
    public IEnumerable<AlbumTrackDAL> Tracks { get; set; }
}