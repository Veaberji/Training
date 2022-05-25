using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.Resources
{
    internal class LastFmArtistsTopLevelDto
    {
        [JsonProperty(PropertyName = "artists")]
        public LastFmArtistsDto TopLevel { get; set; }
    }
}
