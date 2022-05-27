using MusiciansAPP.DAL.WebDataProvider.Resources.Common;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MusiciansAPP.DAL.WebDataProvider.Resources.SimilarArtists
{
    internal class LastFmSimilarArtistDto
    {
        public LastFmSimilarArtistDto()
        {
            Images = new List<LastFmImageDto>();
        }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "playcount")]
        public int Playcount { get; set; }

        [JsonProperty(PropertyName = "listeners")]
        public int Listeners { get; set; }

        [JsonProperty(PropertyName = "mbid")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "streamable")]
        public int Streamable { get; set; }

        [JsonProperty(PropertyName = "image")]
        public IEnumerable<LastFmImageDto> Images { get; set; }
    }
}
