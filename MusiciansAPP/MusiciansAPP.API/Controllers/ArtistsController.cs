using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusiciansAPP.API.Services.Interfaces;
using MusiciansAPP.API.Utils;
using MusiciansAPP.BL.ArtistsService.Interfaces;
using MusiciansAPP.BL.ArtistsService.Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArtistDetailsDto = MusiciansAPP.API.Resources.ArtistDetailsDto;
using ArtistDto = MusiciansAPP.API.Resources.ArtistDto;

namespace MusiciansAPP.API.Controllers
{
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
        public async Task<ActionResult<IEnumerable<ArtistDto>>> GetTopArtists(
            [FromQuery] int pageSize, [FromQuery] int page = 1)
        {
            try
            {
                int size = PagingHelper.GetCorrectPageSize(pageSize);
                var artists = await _artistsService.GetTopArtists(size, page);
                var artistsDto = _mapper.Map<IEnumerable<ArtistDto>>(artists);
                return Ok(artistsDto);
            }
            catch (Exception error)
            {
                _errorHandler.HandleError(error, nameof(GetTopArtists));
                return CreateError();
            }
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<ArtistDetailsDto>> GetArtistDetails(
            string name)
        {
            try
            {
                var artist = await _artistsService.GetArtistDetails(name);
                var artistDto =
                    _mapper.Map<ArtistDetailsDto>(artist);
                return Ok(artistDto);
            }
            catch (ArgumentException error)
            {
                return NotFound(error.Message);
            }
            catch (Exception error)
            {
                _errorHandler.HandleError(error, nameof(GetArtistDetails));
                return CreateError();
            }
        }

        [HttpGet("{name}/top-tracks")]
        public async Task<ActionResult<IEnumerable<TrackDto>>> GetArtistTopTracks(
            string name)
        {
            try
            {
                return Ok(await _artistsService.GetArtistTopTracks(name));
            }
            catch (ArgumentException error)
            {
                return NotFound(error.Message);
            }
            catch (Exception error)
            {
                _errorHandler.HandleError(error, nameof(GetArtistTopTracks));
                return CreateError();
            }
        }

        [HttpGet("{name}/top-albums")]
        public async Task<ActionResult<IEnumerable<AlbumDto>>> GetArtistTopAlbums(
            string name)
        {
            try
            {
                return Ok(await _artistsService.GetArtistTopAlbums(name));
            }
            catch (ArgumentException error)
            {
                return NotFound(error.Message);
            }
            catch (Exception error)
            {
                _errorHandler.HandleError(error, nameof(GetArtistTopAlbums));
                return CreateError();
            }
        }

        [HttpGet("{name}/similar")]
        public async Task<ActionResult<IEnumerable<ArtistDto>>> GetSimilarArtists(
            string name)
        {
            try
            {
                return Ok(await _artistsService.GetSimilarArtists(name));
            }
            catch (ArgumentException error)
            {
                return NotFound(error.Message);
            }
            catch (Exception error)
            {
                _errorHandler.HandleError(error, nameof(GetSimilarArtists));
                return CreateError();
            }
        }

        private ObjectResult CreateError()
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "A problem happened while handling your request.");
        }
    }
}
