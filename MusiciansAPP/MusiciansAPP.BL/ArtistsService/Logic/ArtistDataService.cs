using AutoMapper;
using MusiciansAPP.BL.ArtistsService.BLModels;
using MusiciansAPP.BL.ArtistsService.Interfaces;
using MusiciansAPP.DAL.DBDataProvider.Interfaces;
using MusiciansAPP.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusiciansAPP.BL.ArtistsService.Logic;

public class ArtistDataService : IArtistDataService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ArtistDataService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task SaveTopArtistsAsync(IEnumerable<ArtistBL> artists)
    {
        var mappedArtists = _mapper.Map<IEnumerable<Artist>>(artists);
        await _unitOfWork.Artists.AddOrUpdateRangeAsync(mappedArtists);

        await _unitOfWork.CompleteAsync();
    }

    public async Task SaveArtistDetailsAsync(ArtistDetailsBL artist)
    {
        var mappedArtist = _mapper.Map<Artist>(artist);
        await _unitOfWork.Artists.AddOrUpdateAsync(mappedArtist);

        await _unitOfWork.CompleteAsync();
    }

    public async Task SaveSimilarArtistsAsync(SimilarArtistsBL artists)
    {
        var mappedSimilarArtists = _mapper.Map<IEnumerable<Artist>>(artists.Artists);
        await _unitOfWork.Artists
            .AddOrUpdateSimilarArtistsAsync(artists.ArtistName, mappedSimilarArtists);

        await _unitOfWork.CompleteAsync();
    }
}