using AutoMapper;
using MusiciansAPP.BL.ArtistsService.Resources;
using MusiciansAPP.DAL.WebDataProvider.Resources.ArtistDetails;
using MusiciansAPP.DAL.WebDataProvider.Resources.SimilarArtists;
using MusiciansAPP.DAL.WebDataProvider.Resources.TopArtists;

namespace MusiciansAPP.DAL.WebDataProvider.AutoMapperProfiles
{
    public class ArtistProfile : Profile
    {
        private const string DefaultArtistImage = "https://i.ibb.co/6H89Zzh/default-artist.jpg";
        public ArtistProfile()
        {
            CreateMap<LastFmArtistDto, ArtistDto>()
                .ForMember(dest => dest.ImageUrl,
                    opt => opt.MapFrom(src => DefaultArtistImage));

            CreateMap<LastFmArtistDetailsDto, ArtistDetailsDto>()
                .ForMember(dest => dest.ImageUrl,
                    opt => opt.MapFrom(src => DefaultArtistImage))
                .ForMember(dest => dest.Biography,
                    o => o.MapFrom(src => src.Biography.Content));

            CreateMap<LastFmSimilarArtistDto, ArtistDto>()
                .ForMember(dest => dest.ImageUrl,
                    opt => opt.MapFrom(src => DefaultArtistImage));

            CreateMap<LastFmSimilarArtistsDto, SimilarArtistDto>()
                .ForMember(dest => dest.ArtistName,
                    opt => opt.MapFrom(src => src.MetaData.ArtistName))
                .ForMember(dest => dest.Artists,
                    opt => opt.MapFrom(src => src.Artists));
        }
    }
}
