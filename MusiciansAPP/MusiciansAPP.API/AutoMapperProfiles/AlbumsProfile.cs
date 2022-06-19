using AutoMapper;
using MusiciansAPP.API.UIModels;
using MusiciansAPP.BL.Services.Albums.BLModels;
using MusiciansAPP.BL.Services.Artists.BLModels;

namespace MusiciansAPP.API.AutoMapperProfiles;

public class AlbumsProfile : Profile
{
    public AlbumsProfile()
    {
        CreateMap<AlbumBL, AlbumUI>();
        CreateMap<AlbumDetailsBL, AlbumDetailsUI>();

    }
}