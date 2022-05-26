using Newtonsoft.Json;
using System.Collections.Generic;

namespace MusiciansAPP.DAL.WebDataProvider.Resources.TopArtists
{
    internal class LastFmArtistDto
    {
        public LastFmArtistDto()
        {
            Images = new List<LastFmArtistImageDto>();
        }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "playcount")]
        public int Playcount { get; set; }

        [JsonProperty(PropertyName = "listeners")]
        public int Listeners { get; set; }

        [JsonProperty(PropertyName = "mbid")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string ProfileUrl { get; set; }

        [JsonProperty(PropertyName = "streamable")]
        public int Streamable { get; set; }

        [JsonProperty(PropertyName = "image")]
        public IEnumerable<LastFmArtistImageDto> Images { get; set; }
    }
}
