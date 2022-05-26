using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.Resources.ArtistDetails
{
    internal class LastFmArtistDetailsTopLevelDto
    {
        [JsonProperty(PropertyName = "artist")]
        public LastFmArtistDetailsDto Artist { get; set; }
    }
}
