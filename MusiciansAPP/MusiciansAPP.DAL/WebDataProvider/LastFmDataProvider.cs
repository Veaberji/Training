using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using MusiciansAPP.DAL.DALModels;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.AlbumDetails;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.ArtistDetails;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.ArtistTopAlbums;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.ArtistTopTracks;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.SimilarArtists;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.TopArtists;
using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider;

public class LastFmDataProvider : IWebDataProvider
{
    private const string BaseUrl = "http://ws.audioscrobbler.com/2.0/";
    private readonly string _apiKey;
    private readonly IMapper _mapper;
    private readonly IHttpClient _httpClient;

    public LastFmDataProvider(string apiKey, IMapper mapper, IHttpClient httpClient)
    {
        _apiKey = apiKey;
        _mapper = mapper;
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<ArtistDAL>> GetTopArtistsAsync(int pageSize, int page)
    {
        var response = await GetTopArtistsResponseAsync(pageSize, page);
        if (!response.IsSuccessStatusCode)
        {
            return new List<ArtistDAL>();
        }

        var content = await GetResponseContentAsync(response);
        var result = JsonConvert.DeserializeObject<LastFmArtistsTopLevelDto>(content);

        // this line was added because last.fm returns the number of artists equal page * pageSize for some pages.
        result.TopLevel.Artists = result.TopLevel.Artists.TakeLast(pageSize);

        return _mapper.Map<IEnumerable<ArtistDAL>>(result.TopLevel.Artists);
    }

    public async Task<ArtistDAL> GetArtistDetailsAsync(string name)
    {
        var content = await GetArtistDetailsContentAsync(name);
        var result = JsonConvert.DeserializeObject<LastFmArtistDetailsTopLevelDto>(content);

        return _mapper.Map<ArtistDAL>(result.Artist);
    }

    public async Task<ArtistTracksDAL> GetArtistTopTracksAsync(string name, int pageSize, int page)
    {
        var content = await GetArtistTopTracksContentAsync(name, pageSize, page);
        var result = JsonConvert.DeserializeObject<LastFmArtistTopTracksTopLevelDto>(content);

        result.TopLevel.Tracks = result.TopLevel.Tracks.TakeLast(pageSize);

        return _mapper.Map<ArtistTracksDAL>(result.TopLevel);
    }

    public async Task<ArtistAlbumsDAL> GetArtistTopAlbumsAsync(string name, int pageSize, int page)
    {
        var content = await GetArtistTopAlbumsContentAsync(name, pageSize, page);
        var result = JsonConvert.DeserializeObject<LastFmArtistTopAlbumsTopLevelDto>(content);

        result.TopLevel.Albums = result.TopLevel.Albums.TakeLast(pageSize);

        return _mapper.Map<ArtistAlbumsDAL>(result.TopLevel);
    }

    public async Task<SimilarArtistsDAL> GetSimilarArtistsAsync(string name, int pageSize, int page)
    {
        var content = await GetSimilarArtistsContentAsync(name, pageSize, page);
        var result = JsonConvert.DeserializeObject<LastFmSimilarArtistsTopLevelDto>(content);

        result.TopLevel.Artists = result.TopLevel.Artists.TakeLast(pageSize);

        return _mapper.Map<SimilarArtistsDAL>(result.TopLevel);
    }

    public async Task<AlbumDAL> GetArtistAlbumDetailsAsync(string artistName, string albumName)
    {
        var response = await GetArtistAlbumDetailsResponseAsync(artistName, albumName);
        if (!response.IsSuccessStatusCode)
        {
            ThrowError($"Album {albumName} by artist {artistName} not found");
        }

        var content = await GetResponseContentAsync(response);

        try
        {
            return GetDeserializedRegularAlbum(content);
        }
        catch (Exception)
        {
            return GetDeserializedOneTrackAlbum(content);
        }
    }

    private static string EscapeName(string name)
    {
        const string ampersand = "%26";
        return name.Replace("&", ampersand);
    }

    private static async Task<string> GetResponseContentAsync(HttpResponseMessage response)
    {
        return await response.Content.ReadAsStringAsync();
    }

    private static void CheckResponseContent(string content, string name)
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

    private static void ThrowError(string message)
    {
        throw new ArgumentException(message);
    }

    private async Task<HttpResponseMessage> GetTopArtistsResponseAsync(int pageSize, int page)
    {
        const string method = "chart.gettopartists";
        var url =
            $"{BaseUrl}?method={method}&page={page}&limit={pageSize}&api_key={_apiKey}&format=json";

        return await GetResponseAsync(url);
    }

    private async Task<string> GetArtistDetailsContentAsync(string name)
    {
        const string method = "artist.getinfo";
        var url =
            $"{BaseUrl}?method={method}&artist={EscapeName(name)}&api_key={_apiKey}&format=json";

        return await GetContentAsync(url, name);
    }

    private async Task<string> GetArtistTopTracksContentAsync(string name, int pageSize, int page)
    {
        const string method = "artist.gettoptracks";
        var url = GetUrlForSupplements(method, name, pageSize, page);

        return await GetContentAsync(url, name);
    }

    private async Task<string> GetArtistTopAlbumsContentAsync(string name, int pageSize, int page)
    {
        const string method = "artist.gettopalbums";
        var url = GetUrlForSupplements(method, name, pageSize, page);

        return await GetContentAsync(url, name);
    }

    private async Task<string> GetSimilarArtistsContentAsync(string name, int pageSize, int page)
    {
        const string method = "artist.getsimilar";
        var url =
            $"{BaseUrl}?method={method}&artist={EscapeName(name)}&limit={pageSize * page}&api_key={_apiKey}&format=json";

        return await GetContentAsync(url, name);
    }

    private async Task<HttpResponseMessage> GetArtistAlbumDetailsResponseAsync(
        string artistName, string albumName)
    {
        const string method = "album.getinfo";
        var url =
            $"{BaseUrl}?method={method}&artist={EscapeName(artistName)}&album={EscapeName(albumName)}&api_key={_apiKey}&format=json";

        return await GetResponseAsync(url);
    }

    private async Task<string> GetContentAsync(string url, string artistName)
    {
        var response = await GetResponseAsync(url);
        var content = await GetResponseContentAsync(response);
        CheckResponseContent(content, artistName);

        return content;
    }

    private string GetUrlForSupplements(string method, string name, int pageSize, int page)
    {
        return $"{BaseUrl}?method={method}&artist={EscapeName(name)}&limit={pageSize}&page={page}&api_key={_apiKey}&format=json";
    }

    private async Task<HttpResponseMessage> GetResponseAsync(string url)
    {
        return await _httpClient.GetAsync(url);
    }

    private AlbumDAL GetDeserializedRegularAlbum(string content)
    {
        var result = JsonConvert
            .DeserializeObject<LastFmArtistAlbumTopLevelDto>(content);

        return _mapper.Map<AlbumDAL>(result.TopLevel);
    }

    private AlbumDAL GetDeserializedOneTrackAlbum(string content)
    {
        var result = JsonConvert
            .DeserializeObject<LastFmArtistAlbumOneTrackTopLevelDto>(content);

        return _mapper.Map<AlbumDAL>(result.TopLevel);
    }
}