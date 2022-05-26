﻿using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.Resources.Common
{
    internal class LastFmArtistImageDto
    {
        [JsonProperty(PropertyName = "#text")]
        public string Text { get; set; } = null!;

        [JsonProperty(PropertyName = "size")]
        public string Size { get; set; }
    }
}