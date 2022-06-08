using AutoMapper;
using MusiciansAPP.Domain;

namespace MusiciansAPP.DAL.DBDataProvider.AutoMapperProfiles;

public class ArtistProfile : Profile
{

    public ArtistProfile()
    {
        CreateMap<Artist, Artist>()
            .ForMember(dest => dest.Id,
                opt => opt.Ignore())
            .ForMember(dest => dest.SimilarArtists,
                opt => opt.Ignore())
            .ForMember(dest => dest.ReverseSimilarArtists,
                opt => opt.Ignore())
            .ForMember(dest => dest.Albums,
                opt => opt.Ignore())
            .ForMember(dest => dest.Tracks,
                opt => opt.Ignore());
    }
}