using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusiciansAPP.API.Resources;
using MusiciansAPP.API.Utils;
using MusiciansAPP.BL.ArtistsService.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusiciansAPP.API.Controllers
{
    [Route("api/artists")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly ILogger<ArtistsController> _logger;
        private readonly IArtistsService _artistsService;
        private readonly IMapper _mapper;

        public ArtistsController(ILogger<ArtistsController> logger,
            IArtistsService artistsService, IMapper mapper)
        {
            _logger = logger;
            _artistsService = artistsService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArtistDto>>> GetTopArtists(
            int pageSize, int page = 1)
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
                _logger.LogError(
                    "Exception while getting Top Artists.",
                    error);
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
