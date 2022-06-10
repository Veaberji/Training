using System.Collections.Generic;

namespace MusiciansAPP.DAL.DALModels;

public class ArtistsPagingDAL
{
    public ArtistsPagingDAL()
    {
        Artists = new List<ArtistDAL>();
    }

    public IEnumerable<ArtistDAL> Artists { get; set; }
    public PagingDataDAL PagingData { get; set; }
}