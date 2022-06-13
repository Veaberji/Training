using AutoMapper;
using MusiciansAPP.BL.ArtistsService.BLModels;
using MusiciansAPP.DAL.DALModels;
using MusiciansAPP.Domain;
using System.Collections.Generic;

namespace MusiciansAPP.BL.ArtistsService.AutoMapperProfiles;

public class ArtistProfile : Profile
{
    private const int TotalTopArtists = 4295628;

    public ArtistProfile()
    {
        CreateMap<ArtistDAL, ArtistBL>();
        CreateMap<ArtistsPagingDAL, ArtistsPagingBL>();
        CreateMap<ArtistBL, Artist>().ReverseMap();

        CreateMap<ArtistDetailsDAL, ArtistDetailsBL>();
        CreateMap<ArtistDetailsBL, Artist>().ReverseMap();

        CreateMap<SimilarArtistsDAL, SimilarArtistsBL>();
        CreateMap<SimilarArtistsBL, Artist>()
            .ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => src.ArtistName));

        CreateMap<IEnumerable<Artist>, ArtistsPagingBL>()
            .ForMember(dest => dest.Artists,
                opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.PagingData,
                opt => opt.MapFrom(src => new PagingDataBL { TotalItems = TotalTopArtists }));
    }
}