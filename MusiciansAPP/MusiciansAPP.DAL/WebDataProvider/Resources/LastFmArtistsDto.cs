using Newtonsoft.Json;
using System.Collections.Generic;

namespace MusiciansAPP.DAL.WebDataProvider.Resources
{
    internal class LastFmArtistsDto
    {
        public LastFmArtistsDto()
        {
            Artists = new List<LastFmArtistDto>();
        }

        [JsonProperty(PropertyName = "artist")]
        public IEnumerable<LastFmArtistDto> Artists { get; set; }

        [JsonProperty(PropertyName = "@attr")]
        public LastFmArtistsMetaDataDto MetaData { get; set; }
    }
}
