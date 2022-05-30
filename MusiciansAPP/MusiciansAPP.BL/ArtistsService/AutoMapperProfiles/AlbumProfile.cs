using AutoMapper;
using MusiciansAPP.BL.ArtistsService.BLModels;
using MusiciansAPP.DAL.DALModels;
using MusiciansAPP.Domain;

namespace MusiciansAPP.BL.ArtistsService.AutoMapperProfiles;

public class AlbumProfile : Profile
{
    public AlbumProfile()
    {
        CreateMap<AlbumBL, Album>();
        CreateMap<AlbumDAL, AlbumBL>();
    }
}