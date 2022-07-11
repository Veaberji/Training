using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusiciansAPP.API.Services;
using MusiciansAPP.API.UIModels;
using MusiciansAPP.BL.Services.Albums;
using MusiciansAPP.BL.Services.Artists;
using MusiciansAPP.BL.Services.Tracks;

namespace MusiciansAPP.API.Controllers;

[Route("api/artists")]
[ApiController]
public class ArtistsController : ControllerBase
{
    private const int DefaultSize = 10;
    private const int DefaultPage = 1;

    private readonly IArtistsService _artistsService;
    private readonly ITracksService _tracksService;
    private readonly IAlbumsService _albumsService;
    private readonly IMapper _mapper;
    private readonly IErrorHandler _errorHandler;
    private readonly PagingHelper _pagingHelper;

    public ArtistsController(
        IArtistsService artistsService,
        ITracksService tracksService,
        IAlbumsService albumsService,
        IMapper mapper,
        IErrorHandler errorHandler,
        PagingHelper pagingHelper)
    {
        _artistsService = artistsService;
        _mapper = mapper;
        _errorHandler = errorHandler;
        _pagingHelper = pagingHelper;
        _albumsService = albumsService;
        _tracksService = tracksService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ArtistUI>>> GetTopArtists(
        [FromQuery] int pageSize, [FromQuery] int page = 1)
    {
        var action = async () =>
        {
            int size = _pagingHelper.GetCorrectPageSize(pageSize);
            var artists = await _artistsService.GetTopArtistsAsync(size, page);
            return _mapper.Map<IEnumerable<ArtistUI>>(artists);
        };

        return await GetDataAsync(action, nameof(GetTopArtists));
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<ArtistUI>> GetArtistDetails(
        string name)
    {
        var action = async () =>
        {
            var artist = await _artistsService.GetArtistDetailsAsync(name);
            return _mapper.Map<ArtistUI>(artist);
        };

        return await GetDataAsync(action, nameof(GetArtistDetails));
    }

    [HttpGet("{name}/top-tracks")]
    public async Task<ActionResult<IEnumerable<TrackUI>>> GetArtistTopTracks(
        string name)
    {
        var action = async () =>
        {
            var blModels = await _tracksService.GetArtistTopTracksAsync(name, DefaultSize, DefaultPage);
            return _mapper.Map<IEnumerable<TrackUI>>(blModels);
        };

        return await GetDataAsync(action, nameof(GetArtistTopTracks));
    }

    [HttpGet("{name}/top-albums")]
    public async Task<ActionResult<IEnumerable<AlbumUI>>> GetArtistTopAlbums(
        string name)
    {
        var action = async () =>
        {
            var blModels = await _albumsService.GetArtistTopAlbumsAsync(name, DefaultSize, DefaultPage);
            return _mapper.Map<IEnumerable<AlbumUI>>(blModels);
        };

        return await GetDataAsync(action, nameof(GetArtistTopAlbums));
    }

    [HttpGet("{name}/similar")]
    public async Task<ActionResult<IEnumerable<ArtistUI>>> GetSimilarArtists(
        string name)
    {
        var action = async () =>
        {
            var blModels = await _artistsService.GetSimilarArtistsAsync(name, DefaultSize, DefaultPage);
            return _mapper.Map<IEnumerable<ArtistUI>>(blModels);
        };

        return await GetDataAsync(action, nameof(GetSimilarArtists));
    }

    [HttpGet("{artistName}/album-details/{albumName}")]
    public async Task<ActionResult<AlbumUI>> GetArtistAlbumDetails(
        string artistName, string albumName)
    {
        var action = async () =>
        {
            var blModels = await _albumsService.GetArtistAlbumDetailsAsync(artistName, albumName);
            return _mapper.Map<AlbumUI>(blModels);
        };

        return await GetDataAsync(action, nameof(GetArtistAlbumDetails));
    }

    private async Task<ActionResult<T>> GetDataAsync<T>(Func<Task<T>> action, string method)
    {
        try
        {
            return Ok(await action());
        }
        catch (ArgumentException error)
        {
            return NotFound(error.Message);
        }
        catch (Exception error)
        {
            _errorHandler.HandleError(error, method);
            return CreateError();
        }
    }

    private ObjectResult CreateError()
    {
        return StatusCode(
            StatusCodes.Status500InternalServerError,
            "A problem happened while handling your request.");
    }
}