using MusiciansAPP.DAL.WebDataProvider.Resources.Common;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MusiciansAPP.DAL.WebDataProvider.Resources.ArtistTopAlbums
{
    internal class LastFmArtistTopAlbumDto
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "playcount")]
        public int PlayCount { get; set; }

        [JsonProperty(PropertyName = "image")]
        public IEnumerable<LastFmImageDto> Images { get; set; }
    }
}
