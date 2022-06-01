using AutoMapper;
using MusiciansAPP.BL.ArtistsService.BLModels;
using MusiciansAPP.BL.ArtistsService.Interfaces;
using MusiciansAPP.DAL.DBDataProvider.Interfaces;
using MusiciansAPP.DAL.WebDataProvider.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusiciansAPP.BL.ArtistsService.Logic;

public class ArtistsService : IArtistsService
{
    private readonly IWebDataProvider _webDataProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ArtistsService(IWebDataProvider webDataProvider, IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _webDataProvider = webDataProvider;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ArtistBL>> GetTopArtistsAsync(int pageSize, int page)
    {
        var topArtists = await _webDataProvider.GetTopArtistsAsync(pageSize, page);
        return _mapper.Map<IEnumerable<ArtistBL>>(topArtists);
    }

    public async Task<ArtistDetailsBL> GetArtistDetailsAsync(string name)
    {
        var artistDetails = await _webDataProvider.GetArtistDetailsAsync(name);
        return _mapper.Map<ArtistDetailsBL>(artistDetails);
    }

    public async Task<IEnumerable<TrackBL>> GetArtistTopTracksAsync(string name)
    {
        var artistTracks = await _webDataProvider.GetArtistTopTracksAsync(name);
        return _mapper.Map<IEnumerable<TrackBL>>(artistTracks.Tracks);
    }

    public async Task<IEnumerable<AlbumBL>> GetArtistTopAlbumsAsync(string name)
    {
        var artistAlbums = await _webDataProvider.GetArtistTopAlbumsAsync(name);
        return _mapper.Map<IEnumerable<AlbumBL>>(artistAlbums.Albums);
    }

    public async Task<IEnumerable<ArtistBL>> GetSimilarArtistsAsync(string name)
    {
        var similarArtists = await _webDataProvider.GetSimilarArtistsAsync(name);
        return _mapper.Map<IEnumerable<ArtistBL>>(similarArtists.Artists);
    }

    public async Task<AlbumDetailsBL> GetArtistAlbumAsync(string artistName, string albumName)
    {
        var albumDetails = await _webDataProvider.GetArtistAlbumAsync(artistName, albumName);
        return _mapper.Map<AlbumDetailsBL>(albumDetails);
    }
}