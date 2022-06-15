using System.Collections.Generic;

namespace MusiciansAPP.BL.Services.Artists.BLModels;

public class ArtistsPagingBL
{
    public ArtistsPagingBL()
    {
        Artists = new List<ArtistBL>();
    }

    public IEnumerable<ArtistBL> Artists { get; set; }
    public PagingDataBL PagingData { get; set; }
}