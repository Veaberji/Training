using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.Resources.ArtistDetails
{
    internal class LastFmArtistBiographyDto
    {
        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }
    }
}
