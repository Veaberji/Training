using System.Collections.Generic;

namespace MusiciansAPP.DAL.DALModels;

public class SimilarArtistDAL
{
    public SimilarArtistDAL()
    {
        Artists = new List<ArtistDAL>();
    }

    public string ArtistName { get; set; }
    public IEnumerable<ArtistDAL> Artists { get; set; }
}