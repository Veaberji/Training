using System.Collections.Generic;

namespace MusiciansAPP.BL.Services.Tracks.BLModels;

public class ArtistTracksBL
{
    public ArtistTracksBL()
    {
        Tracks = new List<TrackBL>();
    }

    public string ArtistName { get; set; }
    public IEnumerable<TrackBL> Tracks { get; set; }
}