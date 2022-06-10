using AutoMapper;
using MusiciansAPP.API.UIModels;
using MusiciansAPP.BL.ArtistsService.BLModels;

namespace MusiciansAPP.API.AutoMapperProfiles;

public class ArtistProfile : Profile
{
    public ArtistProfile()
    {
        CreateMap<ArtistDetailsBL, ArtistDetailsUI>();
        CreateMap<ArtistBL, ArtistUI>();
        CreateMap<ArtistsPagingBL, ArtistsPagingUI>();
    }
}