using AutoMapper;
using MusiciansAPP.BL.ArtistsService.Interfaces;
using MusiciansAPP.BL.ArtistsService.Resources;
using MusiciansAPP.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusiciansAPP.BL.ArtistsService.Logic
{
    public class ArtistsService : IArtistsService
    {
        private readonly IWebDataProvider _webDataProvider;
        private readonly IMapper _mapper;

        public ArtistsService(IWebDataProvider webDataProvider, IMapper mapper)
        {
            _webDataProvider = webDataProvider;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Artist>> GetTopArtists(int pageSize, int page)
        {
            var artistsDTos = await _webDataProvider.GetTopArtists(pageSize, page);
            return _mapper.Map<IEnumerable<Artist>>(artistsDTos);
        }

        public async Task<Artist> GetArtistDetails(string name)
        {
            var artistDTo = await _webDataProvider.GetArtistDetails(name);
            return _mapper.Map<Artist>(artistDTo);
        }

        public async Task<ArtistTracksDto> GetArtistTopTracks(string name)
        {
            return await _webDataProvider.GetArtistTopTracks(name);
        }

        public async Task<ArtistAlbumsDto> GetArtistTopAlbums(string name)
        {
            return await _webDataProvider.GetArtistTopAlbums(name);
        }

        public async Task<SimilarArtistDto> GetSimilarArtists(string name)
        {
            return await _webDataProvider.GetSimilarArtists(name);
        }
    }
}
