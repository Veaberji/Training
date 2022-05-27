using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.Resources.Common
{
    internal class LastFmImageDto
    {
        [JsonProperty(PropertyName = "#text")]
        public string Url { get; set; } = null!;

        [JsonProperty(PropertyName = "size")]
        public string Size { get; set; }
    }
}
