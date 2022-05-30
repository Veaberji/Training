using AutoMapper;
using MusiciansAPP.BL.ArtistsService.BLModels;
using MusiciansAPP.DAL.DALModels;
using MusiciansAPP.Domain;

namespace MusiciansAPP.BL.ArtistsService.AutoMapperProfiles;

public class ArtistProfile : Profile
{
    public ArtistProfile()
    {
        CreateMap<ArtistBL, Artist>();
        CreateMap<ArtistDetailsDAL, ArtistDetailsBL>();
        CreateMap<ArtistDAL, ArtistBL>();
    }
}