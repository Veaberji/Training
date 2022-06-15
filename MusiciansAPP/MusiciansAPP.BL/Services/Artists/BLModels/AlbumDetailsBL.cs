using System.Collections.Generic;

namespace MusiciansAPP.BL.Services.Artists.BLModels;

public class AlbumDetailsBL
{
    public AlbumDetailsBL()
    {
        Tracks = new List<AlbumTrackBL>();
    }

    public string Name { get; set; }
    public string ArtistName { get; set; }
    public string ImageUrl { get; set; }
    public IEnumerable<AlbumTrackBL> Tracks { get; set; }
}