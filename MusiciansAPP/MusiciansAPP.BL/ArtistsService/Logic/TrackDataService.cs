using AutoMapper;
using MusiciansAPP.BL.ArtistsService.BLModels;
using MusiciansAPP.BL.ArtistsService.Interfaces;
using MusiciansAPP.DAL.DBDataProvider.Interfaces;
using MusiciansAPP.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusiciansAPP.BL.ArtistsService.Logic;

public class TrackDataService : ITrackDataService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TrackDataService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task SaveTopTracksAsync(ArtistTracksBL tracks)
    {
        var mappedTracks = _mapper.Map<IEnumerable<Track>>(tracks.Tracks);
        var artist = await GetOrCreateArtistAsync(tracks.ArtistName);
        await _unitOfWork.Tracks
            .AddOrUpdateArtistTracksAsync(artist, mappedTracks);
        await _unitOfWork.CompleteAsync();
    }


    public async Task UpdateAlbumTracksAsync(Album album)
    {
        await _unitOfWork.Tracks
            .AddOrUpdateAlbumTracksAsync(album);
        await _unitOfWork.CompleteAsync();
    }

    private async Task<Artist> GetOrCreateArtistAsync(string artistName)
    {
        return await _unitOfWork.Artists
            .GetArtistDetailsAsync(artistName) ?? new Artist
            {
                Name = artistName
            };
    }
}