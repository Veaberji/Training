using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.Resources.ArtistTopAlbums
{
    internal class LastFmArtistTopAlbumDto
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "playcount")]
        public int PlayCount { get; set; }
    }
}
