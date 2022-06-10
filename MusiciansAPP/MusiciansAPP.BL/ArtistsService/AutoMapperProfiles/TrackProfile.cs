using AutoMapper;
using MusiciansAPP.BL.ArtistsService.BLModels;
using MusiciansAPP.DAL.DALModels;
using MusiciansAPP.Domain;

namespace MusiciansAPP.BL.ArtistsService.AutoMapperProfiles;

public class TrackProfile : Profile
{
    public TrackProfile()
    {
        CreateMap<TrackDAL, TrackBL>();
        CreateMap<TrackBL, Track>();

        CreateMap<AlbumTrackDAL, AlbumTrackBL>();
        CreateMap<AlbumTrackBL, Track>();

        CreateMap<ArtistTracksDAL, ArtistTracksBL>();
    }
}