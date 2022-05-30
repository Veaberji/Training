using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusiciansAPP.API.Services.Interfaces;
using MusiciansAPP.API.UIModels;
using MusiciansAPP.API.Utils;
using MusiciansAPP.BL.ArtistsService.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusiciansAPP.API.Controllers;

[Route("api/artists")]
[ApiController]
public class ArtistsController : ControllerBase
{
    private readonly IArtistsService _artistsService;
    private readonly IMapper _mapper;
    private readonly IErrorHandler _errorHandler;
    public ArtistsController(IArtistsService artistsService, IMapper mapper,
        IErrorHandler errorHandler)
    {
        _artistsService = artistsService;
        _mapper = mapper;
        _errorHandler = errorHandler;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ArtistUI>>> GetTopArtists(
        [FromQuery] int pageSize, [FromQuery] int page = 1)
    {
        var action = async () =>
        {
            int size = PagingHelper.GetCorrectPageSize(pageSize);
            var artists = await _artistsService.GetTopArtistsAsync(size, page);
            return _mapper.Map<IEnumerable<ArtistUI>>(artists);
        };

        return await GetDataAsync(action, nameof(GetArtistDetails));
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<ArtistDetailsUI>> GetArtistDetails(
        string name)
    {
        var action = async () =>
        {
            var artist = await _artistsService.GetArtistDetailsAsync(name);
            return _mapper.Map<ArtistDetailsUI>(artist);
        };

        return await GetDataAsync(action, nameof(GetArtistDetails));
    }

    [HttpGet("{name}/top-tracks")]
    public async Task<ActionResult<IEnumerable<TrackUI>>> GetArtistTopTracks(
        string name)
    {
        var action = async () =>
        {
            var blModels = await _artistsService.GetArtistTopTracksAsync(name);
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
            var blModels = await _artistsService.GetArtistTopAlbumsAsync(name);
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
            var blModels = await _artistsService.GetSimilarArtistsAsync(name);
            return _mapper.Map<IEnumerable<ArtistUI>>(blModels);
        };

        return await GetDataAsync(action, nameof(GetSimilarArtists));
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
        return StatusCode(StatusCodes.Status500InternalServerError,
            "A problem happened while handling your request.");
    }
}