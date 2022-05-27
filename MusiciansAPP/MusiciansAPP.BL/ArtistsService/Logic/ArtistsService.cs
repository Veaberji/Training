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

        public async Task<IEnumerable<TrackDto>> GetArtistTopTracks(string name)
        {
            var artistTracks = await _webDataProvider.GetArtistTopTracks(name);
            return artistTracks.Tracks;
        }

        public async Task<IEnumerable<AlbumDto>> GetArtistTopAlbums(string name)
        {
            var artistAlbums = await _webDataProvider.GetArtistTopAlbums(name);
            return artistAlbums.Albums;
        }

        public async Task<IEnumerable<ArtistDto>> GetSimilarArtists(string name)
        {
            var similarArtists = await _webDataProvider.GetSimilarArtists(name);
            return similarArtists.Artists;
        }
    }
}
