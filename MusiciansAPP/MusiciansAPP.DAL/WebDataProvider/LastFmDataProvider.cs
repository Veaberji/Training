using AutoMapper;
using MusiciansAPP.BL.ArtistsService.Interfaces;
using MusiciansAPP.BL.ArtistsService.Resources;
using MusiciansAPP.DAL.WebDataProvider.Resources.ArtistDetails;
using MusiciansAPP.DAL.WebDataProvider.Resources.TopArtists;
using Newtonsoft.Json;
using System;
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
        private const string LastFmNameSeparator = "+";

        public LastFmDataProvider(string apiKey, IMapper mapper)
        {
            _apiKey = apiKey;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ArtistDto>> GetTopArtists(int pageSize, int page)
        {
            const string method = "chart.gettopartists";
            var url = $"{BaseUrl}?method={method}&page={page}&limit={pageSize}&api_key={_apiKey}&format=json";

            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return new List<ArtistDto>();
            string content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert
                .DeserializeObject<LastFmArtistsTopLevelDto>(content);
            var artists = _mapper.Map<IEnumerable<ArtistDto>>(result.TopLevel.Artists);
            return artists;

        }

        public async Task<ArtistDetailsDto> GetArtistDetails(string name)
        {
            const string method = "artist.getinfo";

            var url = $"{BaseUrl}?method={method}&artist={name}&api_key={_apiKey}&format=json";

            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);

            string content = await response.Content.ReadAsStringAsync();
            if (!IsArtistFound(content))
            {
                throw new ArgumentException($"Artist {name} not found");
            }
            var result = JsonConvert
                .DeserializeObject<LastFmArtistDetailsTopLevelDto>(content);

            var artist = _mapper.Map<ArtistDetailsDto>(result.Artist);
            return artist;

        }

        private static bool IsArtistFound(string content)
        {
            return !content.Contains("error");
        }
    }
}
