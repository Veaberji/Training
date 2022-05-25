using AutoMapper;
using MusiciansAPP.BL.ArtistsService.Interfaces;
using MusiciansAPP.DAL.WebDataProvider.Resources;
using MusiciansAPP.Domain;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MusiciansAPP.DAL.WebDataProvider
{
    public class LastFmDataProvider : IWebDataProvider
    {
        private const string BaseUrl = "http://ws.audioscrobbler.com/2.0/";
        private readonly string _apiKey;
        private readonly IMapper _mapper;

        public LastFmDataProvider(string apiKey, IMapper mapper)
        {
            _apiKey = apiKey;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Artist>> GetTopArtists(int pageSize, int page)
        {
            const string method = "chart.gettopartists";
            var url = $"{BaseUrl}?method={method}&page={page}&limit={pageSize}&api_key={_apiKey}&format=json";

            //todo: interface
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<LastFmArtistsTopLevelDto>(apiResponse);

                    var artists = _mapper.Map<IEnumerable<Artist>>(result.TopLevel.Artists);
                    return artists;
                }
            }
            return null;
        }
    }
}
