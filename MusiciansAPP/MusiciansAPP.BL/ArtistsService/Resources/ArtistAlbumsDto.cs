using System.Collections.Generic;

namespace MusiciansAPP.BL.ArtistsService.Resources
{
    public class ArtistAlbumsDto
    {
        public ArtistAlbumsDto()
        {
            Albums = new List<AlbumDto>();
        }

        public string ArtistName { get; set; }
        public IEnumerable<AlbumDto> Albums { get; set; }
    }
}
