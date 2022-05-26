using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.Resources.TopArtists
{
    internal class LastFmArtistsTopLevelDto
    {
        [JsonProperty(PropertyName = "artists")]
        public LastFmArtistsDto TopLevel { get; set; }
    }
}
