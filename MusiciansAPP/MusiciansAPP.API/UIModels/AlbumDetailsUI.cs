using System.Collections.Generic;

namespace MusiciansAPP.API.UIModels;

public class AlbumDetailsUI
{
    public AlbumDetailsUI()
    {
        Tracks = new List<AlbumTrackUI>();
    }

    public string Name { get; set; }
    public string ArtistName { get; set; }
    public string ImageUrl { get; set; }
    public IEnumerable<AlbumTrackUI> Tracks { get; set; }
}