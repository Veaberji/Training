using AutoMapper;
using MusiciansAPP.BL.Services.Artists.BLModels;
using MusiciansAPP.DAL.DALModels;
using MusiciansAPP.Domain;

namespace MusiciansAPP.BL.Services.Artists.AutoMapperProfiles;

public class AlbumProfile : Profile
{
    public AlbumProfile()
    {
        CreateMap<AlbumDAL, AlbumBL>();
        CreateMap<AlbumBL, Album>().ReverseMap();

        CreateMap<AlbumDetailsDAL, AlbumDetailsBL>();
        CreateMap<AlbumDetailsBL, Album>().ReverseMap();

        CreateMap<ArtistAlbumsDAL, ArtistAlbumsBL>();
    }
}