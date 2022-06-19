using AutoMapper;
using MusiciansAPP.API.UIModels;
using MusiciansAPP.BL.Services.Albums.BLModels;
using MusiciansAPP.BL.Services.Tracks.BLModels;

namespace MusiciansAPP.API.AutoMapperProfiles;

public class TracksProfile : Profile
{
    public TracksProfile()
    {
        CreateMap<TrackBL, TrackUI>();
        CreateMap<AlbumTrackBL, AlbumTrackUI>();
    }
}