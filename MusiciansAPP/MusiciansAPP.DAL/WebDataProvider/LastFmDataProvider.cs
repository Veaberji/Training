using AutoMapper;
using MusiciansAPP.DAL.DALModels;
using MusiciansAPP.DAL.WebDataProvider.Interfaces;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.AlbumDetails;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.ArtistDetails;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.ArtistTopAlbums;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.ArtistTopTracks;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.SimilarArtists;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.TopArtists;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MusiciansAPP.DAL.WebDataProvider;

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

    public async Task<ArtistsPagingDAL> GetTopArtistsAsync(int pageSize, int page)
    {
        const string method = "chart.gettopartists";
        var url = $"{BaseUrl}?method={method}&page={page}&limit={pageSize}&api_key={_apiKey}&format=json";
        var response = await GetResponseAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            return new ArtistsPagingDAL();
        }

        string content = await GetResponseContentAsync(response);

        var result = JsonConvert
            .DeserializeObject<LastFmArtistsTopLevelDto>(content);

        //this line was added because last.fm returns the number of artists equal page * pageSize for some pages.
        result.TopLevel.Artists = result.TopLevel.Artists.TakeLast(pageSize);

        return _mapper.Map<ArtistsPagingDAL>(result.TopLevel);
    }

    public async Task<ArtistDetailsDAL> GetArtistDetailsAsync(string name)
    {
        const string method = "artist.getinfo";
        var url = $"{BaseUrl}?method={method}&artist={name}&api_key={_apiKey}&format=json";
        string content = await GetContentAsync(url, name);

        var result = JsonConvert
            .DeserializeObject<LastFmArtistDetailsTopLevelDto>(content);

        return _mapper.Map<ArtistDetailsDAL>(result.Artist);
    }

    public async Task<ArtistTracksDAL> GetArtistTopTracksAsync(string name, int pageSize, int page)
    {
        const string method = "artist.gettoptracks";
        var url = GetUrlForSupplements(method, name, pageSize, page);
        string content = await GetContentAsync(url, name);

        var result = JsonConvert
            .DeserializeObject<LastFmArtistTopTracksTopLevelDto>(content);
        result.TopLevel.Tracks = result.TopLevel.Tracks.TakeLast(pageSize);

        return _mapper.Map<ArtistTracksDAL>(result.TopLevel);
    }

    public async Task<ArtistAlbumsDAL> GetArtistTopAlbumsAsync(string name, int pageSize, int page)
    {
        const string method = "artist.gettopalbums";
        var url = GetUrlForSupplements(method, name, pageSize, page);
        string content = await GetContentAsync(url, name);

        var result = JsonConvert
            .DeserializeObject<LastFmArtistTopAlbumsTopLevelDto>(content);
        result.TopLevel.Albums = result.TopLevel.Albums.TakeLast(pageSize);

        return _mapper.Map<ArtistAlbumsDAL>(result.TopLevel);
    }

    public async Task<SimilarArtistsDAL> GetSimilarArtistsAsync(string name, int pageSize, int page)
    {
        const string method = "artist.getsimilar";
        var url = $"{BaseUrl}?method={method}&artist={name}&limit={pageSize * page}&api_key={_apiKey}&format=json";
        string content = await GetContentAsync(url, name);

        var result = JsonConvert
            .DeserializeObject<LastFmSimilarArtistsTopLevelDto>(content);
        result.TopLevel.Artists = result.TopLevel.Artists.TakeLast(pageSize);

        return _mapper.Map<SimilarArtistsDAL>(result.TopLevel);
    }

    public async Task<AlbumDetailsDAL> GetArtistAlbumDetailsAsync(string artistName, string albumName)
    {
        const string method = "album.getinfo";
        var url = $"{BaseUrl}?method={method}&artist={artistName}&album={albumName}&api_key={_apiKey}&format=json";
        var response = await GetResponseAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            ThrowError($"Album {albumName} by artist {artistName} not found");
        }

        string content = await GetResponseContentAsync(response);

        var result = JsonConvert
            .DeserializeObject<LastFmArtistAlbumTopLevelDto>(content);

        return _mapper.Map<AlbumDetailsDAL>(result.TopLevel);
    }

    private async Task<string> GetContentAsync(string url, string artistName)
    {
        var response = await GetResponseAsync(url);
        string content = await GetResponseContentAsync(response);
        CheckResponseContent(content, artistName);

        return content;
    }

    private string GetUrlForSupplements(string method, string name, int pageSize, int page)
    {
        return $"{BaseUrl}?method={method}&artist={name}&limit={pageSize}&page={page}&api_key={_apiKey}&format=json";
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

    private void CheckResponseContent(string content, string name)
    {
        if (!IsArtistFound(content))
        {
            ThrowError($"Artist {name} not found");
        }
    }

    private static bool IsArtistFound(string content)
    {
        const string lastFmErrorMessage = "The artist you supplied could not be found";
        return !content.Contains(lastFmErrorMessage);
    }

    private void ThrowError(string message)
    {
        throw new ArgumentException(message);
    }
}