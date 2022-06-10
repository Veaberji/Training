using System.Collections.Generic;

namespace MusiciansAPP.API.UIModels;

public class ArtistsPagingUI
{
    public ArtistsPagingUI()
    {
        Artists = new List<ArtistUI>();
    }

    public IEnumerable<ArtistUI> Artists { get; set; }
    public PagingDataUI PagingData { get; set; }
}