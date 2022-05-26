using System.Collections.Generic;

namespace MusiciansAPP.BL.ArtistsService.Resources
{
    public class ArtistTracksDto
    {
        public ArtistTracksDto()
        {
            Tracks = new List<TrackDto>();
        }

        public string ArtistName { get; set; }
        public IEnumerable<TrackDto> Tracks { get; set; }
    }
}
