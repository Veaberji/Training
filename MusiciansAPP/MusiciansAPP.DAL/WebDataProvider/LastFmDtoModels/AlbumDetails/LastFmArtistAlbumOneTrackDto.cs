using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.Common;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.AlbumDetails;

internal class LastFmArtistAlbumOneTrackDto
{
    public LastFmArtistAlbumOneTrackDto()
    {
        Images = new List<LastFmImageDto>();
    }

    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    [JsonProperty(PropertyName = "artist")]
    public string ArtistName { get; set; }

    [JsonProperty(PropertyName = "image")]
    public IEnumerable<LastFmImageDto> Images { get; set; }

    [JsonProperty(PropertyName = "tracks")]
    public LastFmArtistAlbumTrackDto Track { get; set; }
}