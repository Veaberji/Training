using AutoMapper;
using MusiciansAPP.BL.ArtistsService.BLModels;
using MusiciansAPP.DAL.DALModels;
using MusiciansAPP.Domain;

namespace MusiciansAPP.BL.ArtistsService.AutoMapperProfiles;

public class TrackProfile : Profile
{
    public TrackProfile()
    {
        CreateMap<TrackBL, Track>();
        CreateMap<TrackDAL, TrackBL>();
    }
}