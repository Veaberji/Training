using System.Collections.Generic;

namespace MusiciansAPP.BL.ArtistsService.Resources
{
    public class SimilarArtistDto
    {
        public SimilarArtistDto()
        {
            Artists = new List<ArtistDto>();
        }

        public string ArtistName { get; set; }
        public IEnumerable<ArtistDto> Artists { get; set; }
    }
}
