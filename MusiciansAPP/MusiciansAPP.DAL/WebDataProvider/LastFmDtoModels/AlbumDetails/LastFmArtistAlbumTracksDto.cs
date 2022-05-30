using Newtonsoft.Json;
using System.Collections.Generic;

namespace MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.AlbumDetails;

internal class LastFmArtistAlbumTracksDto
{
    public LastFmArtistAlbumTracksDto()
    {
        Tracks = new List<LastFmAlbumTrackDto>();
    }

    [JsonProperty(PropertyName = "track")]
    public IEnumerable<LastFmAlbumTrackDto> Tracks { get; set; }
}