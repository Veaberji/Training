using AutoMapper;
using MusiciansAPP.BL.Services.Tracks.BLModels;
using MusiciansAPP.BL.Services.Tracks.Interfaces;
using MusiciansAPP.DAL.DBDataProvider.Interfaces;
using MusiciansAPP.DAL.WebDataProvider.Interfaces;
using MusiciansAPP.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusiciansAPP.BL.Services.Tracks.Logic;

public class TracksService : ITracksService
{
    private readonly IWebDataProvider _webDataProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TracksService(IWebDataProvider webDataProvider,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _webDataProvider = webDataProvider;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<TrackBL>> GetArtistTopTracksAsync(
        string name, int pageSize, int page)
    {
        var tracksFromDb = await _unitOfWork.Tracks
            .GetTopTracksForArtistAsync(name, pageSize, page);
        if (Entity.IsFullData(tracksFromDb, pageSize))
        {
            return _mapper.Map<IEnumerable<TrackBL>>(tracksFromDb);
        }

        var artistTracksDAL = await _webDataProvider.GetArtistTopTracksAsync(name, pageSize, page);
        var artistTracksBL = _mapper.Map<ArtistTracksBL>(artistTracksDAL);

        await SaveArtistTopTracksAsync(artistTracksBL);

        return artistTracksBL.Tracks;
    }

    private async Task SaveArtistTopTracksAsync(ArtistTracksBL model)
    {
        var tracks = _mapper.Map<IEnumerable<Track>>(model.Tracks);
        var artist =
            await _unitOfWork.Artists.GetArtistDetailsAsync(model.ArtistName);
        await _unitOfWork.Tracks
            .AddOrUpdateArtistTracksAsync(artist, tracks);
        await _unitOfWork.CompleteAsync();
    }
}