using AutoMapper;
using MusiciansAPP.BL.ArtistsService.BLModels;
using MusiciansAPP.DAL.DALModels;
using MusiciansAPP.Domain;

namespace MusiciansAPP.BL.ArtistsService.AutoMapperProfiles;

public class ArtistProfile : Profile
{
    public ArtistProfile()
    {
        CreateMap<ArtistDAL, ArtistBL>();
        CreateMap<ArtistBL, Artist>();

        CreateMap<ArtistDetailsDAL, ArtistDetailsBL>();
        CreateMap<ArtistDetailsBL, Artist>();

        CreateMap<SimilarArtistsDAL, SimilarArtistsBL>();
        CreateMap<SimilarArtistsBL, Artist>()
            .ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => src.ArtistName));
    }
}