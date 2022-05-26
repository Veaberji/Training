using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.Resources.ArtistTopTracks
{
    internal class LastFmArtistTopTracksTopLevelDto
    {
        [JsonProperty(PropertyName = "toptracks")]
        public LastFmArtistTopTracksDto TopLevel { get; set; }
    }
}
