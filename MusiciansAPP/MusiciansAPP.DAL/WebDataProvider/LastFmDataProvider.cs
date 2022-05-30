using AutoMapper;
using MusiciansAPP.DAL.DALModels;
using MusiciansAPP.DAL.WebDataProvider.Interfaces;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.ArtistDetails;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.ArtistTopAlbums;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.ArtistTopTracks;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.SimilarArtists;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.TopArtists;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MusiciansAPP.DAL.WebDataProvider;

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

    public async Task<IEnumerable<ArtistDAL>> GetTopArtistsAsync(int pageSize, int page)
    {
        const string method = "chart.gettopartists";
        var url = $"{BaseUrl}?method={method}&page={page}&limit={pageSize}&api_key={_apiKey}&format=json";
        var response = await GetResponseAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            return new List<ArtistDAL>();
        }

        string content = await GetResponseContentAsync(response);

        var result = JsonConvert
            .DeserializeObject<LastFmArtistsTopLevelDto>(content);

        return _mapper.Map<IEnumerable<ArtistDAL>>(result.TopLevel.Artists);
    }

    public async Task<ArtistDetailsDAL> GetArtistDetailsAsync(string name)
    {
        const string method = "artist.getinfo";
        var url = $"{BaseUrl}?method={method}&artist={name}&api_key={_apiKey}&format=json";
        string content = await GetContent(url, name);

        var result = JsonConvert
            .DeserializeObject<LastFmArtistDetailsTopLevelDto>(content);

        return _mapper.Map<ArtistDetailsDAL>(result.Artist);
    }

    public async Task<ArtistTracksDAL> GetArtistTopTracksAsync(string name)
    {
        const string method = "artist.gettoptracks";
        var url = GetUrlForSupplements(method, name);
        string content = await GetContent(url, name);

        var result = JsonConvert
            .DeserializeObject<LastFmArtistTopTracksTopLevelDto>(content);

        return _mapper.Map<ArtistTracksDAL>(result.TopLevel);
    }

    public async Task<ArtistAlbumsDAL> GetArtistTopAlbumsAsync(string name)
    {
        const string method = "artist.gettopalbums";
        var url = GetUrlForSupplements(method, name);
        string content = await GetContent(url, name);

        var result = JsonConvert
            .DeserializeObject<LastFmArtistTopAlbumsTopLevelDto>(content);

        return _mapper.Map<ArtistAlbumsDAL>(result.TopLevel);
    }

    public async Task<SimilarArtistDAL> GetSimilarArtistsAsync(string name)
    {
        const string method = "artist.getsimilar";
        var url = GetUrlForSupplements(method, name);
        string content = await GetContent(url, name);

        var result = JsonConvert
            .DeserializeObject<LastFmSimilarArtistsTopLevelDto>(content);

        return _mapper.Map<SimilarArtistDAL>(result.TopLevel);
    }

    private async Task<string> GetContent(string url, string artistName)
    {
        var response = await GetResponseAsync(url);
        string content = await GetResponseContentAsync(response);
        CheckResponseContent(content, artistName);

        return content;
    }

    private string GetUrlForSupplements(string method, string name)
    {
        return $"{BaseUrl}?method={method}&artist={name}&limit={DefaultSize}&api_key={_apiKey}&format=json";
    }

    private async Task<HttpResponseMessage> GetResponseAsync(string url)
    {
        using var httpClient = new HttpClient();

        return await httpClient.GetAsync(url);
    }

    private async Task<string> GetResponseContentAsync(HttpResponseMessage response)
    {
        return await response.Content.ReadAsStringAsync();
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