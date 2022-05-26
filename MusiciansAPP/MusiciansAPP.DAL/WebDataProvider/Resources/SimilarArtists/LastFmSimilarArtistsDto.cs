using Newtonsoft.Json;
using System.Collections.Generic;

namespace MusiciansAPP.DAL.WebDataProvider.Resources.SimilarArtists
{
    internal class LastFmSimilarArtistsDto
    {
        public LastFmSimilarArtistsDto()
        {
            Artists = new List<LastFmSimilarArtistDto>();
        }

        [JsonProperty(PropertyName = "artist")]
        public IEnumerable<LastFmSimilarArtistDto> Artists { get; set; }

        [JsonProperty(PropertyName = "@attr")]
        public LastFmSimilarArtistMetaDataDto MetaData { get; set; }
    }
}
