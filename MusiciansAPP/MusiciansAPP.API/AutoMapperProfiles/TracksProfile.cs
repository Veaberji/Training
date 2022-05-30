using AutoMapper;
using MusiciansAPP.API.UIModels;
using MusiciansAPP.BL.ArtistsService.BLModels;

namespace MusiciansAPP.API.AutoMapperProfiles;

public class TracksProfile : Profile
{
    public TracksProfile()
    {
        CreateMap<TrackBL, TrackUI>();
        CreateMap<AlbumTrackBL, AlbumTrackUI>();
    }
}