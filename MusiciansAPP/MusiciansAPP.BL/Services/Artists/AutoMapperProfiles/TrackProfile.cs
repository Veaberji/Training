using AutoMapper;
using MusiciansAPP.BL.Services.Artists.BLModels;
using MusiciansAPP.DAL.DALModels;
using MusiciansAPP.Domain;

namespace MusiciansAPP.BL.Services.Artists.AutoMapperProfiles;

public class TrackProfile : Profile
{
    public TrackProfile()
    {
        CreateMap<TrackDAL, TrackBL>();
        CreateMap<TrackBL, Track>().ReverseMap();

        CreateMap<AlbumTrackDAL, AlbumTrackBL>();
        CreateMap<AlbumTrackBL, Track>().ReverseMap();

        CreateMap<ArtistTracksDAL, ArtistTracksBL>();
    }
}