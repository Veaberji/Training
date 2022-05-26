using AutoMapper;
using MusiciansAPP.BL.ArtistsService.Interfaces;
using MusiciansAPP.BL.ArtistsService.Resources;
using MusiciansAPP.DAL.WebDataProvider.Resources.ArtistDetails;
using MusiciansAPP.DAL.WebDataProvider.Resources.ArtistTopAlbums;
using MusiciansAPP.DAL.WebDataProvider.Resources.ArtistTopTracks;
using MusiciansAPP.DAL.WebDataProvider.Resources.SimilarArtists;
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
        private const int DefaultSize = 10;
        private readonly string _apiKey;
        private readonly IMapper _mapper;

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
            return _mapper.Map<IEnumerable<ArtistDto>>(result.TopLevel.Artists);
        }

        public async Task<ArtistDetailsDto> GetArtistDetails(string name)
        {
            const string method = "artist.getinfo";
            var url = $"{BaseUrl}?method={method}&artist={name}&api_key={_apiKey}&format=json";

            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            string content = await response.Content.ReadAsStringAsync();
            CheckResponseContent(content, name);

            var result = JsonConvert
                .DeserializeObject<LastFmArtistDetailsTopLevelDto>(content);
            return _mapper.Map<ArtistDetailsDto>(result.Artist);
        }

        public async Task<ArtistTracksDto> GetArtistTopTracks(string name)
        {
            const string method = "artist.gettoptracks";
            var url = GetUrlForSupplements(method, name);

            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            string content = await response.Content.ReadAsStringAsync();
            CheckResponseContent(content, name);

            var result = JsonConvert
                .DeserializeObject<LastFmArtistTopTracksTopLevelDto>(content);
            return _mapper.Map<ArtistTracksDto>(result.TopLevel);
        }

        public async Task<ArtistAlbumsDto> GetArtistTopAlbums(string name)
        {
            const string method = "artist.gettopalbums";
            var url = GetUrlForSupplements(method, name);

            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            string content = await response.Content.ReadAsStringAsync();
            CheckResponseContent(content, name);

            var result = JsonConvert
                .DeserializeObject<LastFmArtistTopAlbumsTopLevelDto>(content);
            return _mapper.Map<ArtistAlbumsDto>(result.TopLevel);
        }

        public async Task<SimilarArtistDto> GetSimilarArtists(string name)
        {
            const string method = "artist.getsimilar";
            var url = GetUrlForSupplements(method, name);

            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            string content = await response.Content.ReadAsStringAsync();
            CheckResponseContent(content, name);

            var result = JsonConvert
                .DeserializeObject<LastFmSimilarArtistsTopLevelDto>(content);
            return _mapper.Map<SimilarArtistDto>(result.TopLevel);
        }

        private string GetUrlForSupplements(string method, string name)
        {
            return $"{BaseUrl}?method={method}&artist={name}&limit={DefaultSize}&api_key={_apiKey}&format=json";
        }

        private static void CheckResponseContent(string content, string name)
        {
            if (!IsArtistFound(content))
            {
                throw new ArgumentException($"Artist {name} not found");
            }
        }

        private static bool IsArtistFound(string content)
        {
            return !content.Contains("error");
        }
    }
}
