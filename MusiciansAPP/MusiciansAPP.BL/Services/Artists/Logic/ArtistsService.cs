using AutoMapper;
using MusiciansAPP.BL.Services.Artists.BLModels;
using MusiciansAPP.BL.Services.Artists.Interfaces;
using MusiciansAPP.DAL.DBDataProvider.Interfaces;
using MusiciansAPP.DAL.WebDataProvider.Interfaces;
using MusiciansAPP.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusiciansAPP.BL.Services.Artists.Logic;

public class ArtistsService : IArtistsService
{
    private readonly IWebDataProvider _webDataProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ArtistsService(IWebDataProvider webDataProvider,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _webDataProvider = webDataProvider;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ArtistsPagingBL> GetTopArtistsAsync(int pageSize, int page)
    {
        var artistsFromDb = await _unitOfWork.Artists
            .GetTopArtistsAsync(pageSize, page);
        if (Entity.IsFullData(artistsFromDb, pageSize))
        {
            return _mapper.Map<ArtistsPagingBL>(artistsFromDb);
        }

        var topArtistsDAL = await _webDataProvider.GetTopArtistsAsync(pageSize, page);
        var topArtistsBL = _mapper.Map<ArtistsPagingBL>(topArtistsDAL);

        await SaveTopArtistsAsync(topArtistsBL);

        return topArtistsBL;
    }

    public async Task<ArtistDetailsBL> GetArtistDetailsAsync(string name)
    {
        var artistFromDb = await _unitOfWork.Artists.GetArtistDetailsAsync(name);
        if (artistFromDb.IsArtistDetailsUpToDate())
        {
            return _mapper.Map<ArtistDetailsBL>(artistFromDb);
        }

        var artistDetailsDAL = await _webDataProvider.GetArtistDetailsAsync(name);
        var artistDetailsBL = _mapper.Map<ArtistDetailsBL>(artistDetailsDAL);

        await SaveArtistDetailsAsync(artistDetailsBL);

        return artistDetailsBL;
    }

    public async Task<IEnumerable<ArtistBL>> GetSimilarArtistsAsync(
        string name, int pageSize, int page)
    {
        var artistsFromDb = await _unitOfWork.Artists
            .GetArtistWithSimilarAsync(name, pageSize, page);
        if (Entity.IsFullData(artistsFromDb.SimilarArtists, pageSize))
        {
            return _mapper.Map<IEnumerable<ArtistBL>>(artistsFromDb.SimilarArtists);
        }

        var similarArtistsDAL = await _webDataProvider.GetSimilarArtistsAsync(name, pageSize, page);
        var similarArtistsBL = _mapper.Map<SimilarArtistsBL>(similarArtistsDAL);

        await SaveSimilarArtistsAsync(similarArtistsBL);

        return similarArtistsBL.Artists;
    }

    private async Task SaveTopArtistsAsync(ArtistsPagingBL model)
    {
        var artists = _mapper.Map<IEnumerable<Artist>>(model.Artists);
        await _unitOfWork.Artists.AddOrUpdateRangeAsync(artists);
        await _unitOfWork.CompleteAsync();
    }

    private async Task SaveArtistDetailsAsync(ArtistDetailsBL model)
    {
        var artist = _mapper.Map<Artist>(model);
        await _unitOfWork.Artists.AddOrUpdateAsync(artist);
        await _unitOfWork.CompleteAsync();
    }

    private async Task SaveSimilarArtistsAsync(SimilarArtistsBL model)
    {
        var similarArtists =
            _mapper.Map<IEnumerable<Artist>>(model.Artists);
        await _unitOfWork.Artists
            .AddOrUpdateSimilarArtistsAsync(model.ArtistName, similarArtists);
        await _unitOfWork.CompleteAsync();
    }
}