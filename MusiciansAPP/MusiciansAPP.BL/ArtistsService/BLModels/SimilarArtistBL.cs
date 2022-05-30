using System.Collections.Generic;

namespace MusiciansAPP.BL.ArtistsService.BLModels;

public class SimilarArtistBL
{
    public SimilarArtistBL()
    {
        Artists = new List<ArtistBL>();
    }

    public string ArtistName { get; set; }
    public IEnumerable<ArtistBL> Artists { get; set; }
}