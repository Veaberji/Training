using AutoMapper;
using MusiciansAPP.BL.ArtistsService.Resources;
using MusiciansAPP.DAL.WebDataProvider.Resources.ArtistDetails;
using MusiciansAPP.DAL.WebDataProvider.Resources.TopArtists;

namespace MusiciansAPP.DAL.WebDataProvider.AutoMapperProfiles
{
    public class ArtistProfile : Profile
    {
        private const string DefaultArtistImage = "https://i.ibb.co/6H89Zzh/default-artist.jpg";
        public ArtistProfile()
        {
            CreateMap<LastFmArtistDto, ArtistDto>()
                .ForMember(a => a.ImageUrl,
                    o => o.MapFrom(l => DefaultArtistImage));

            CreateMap<LastFmArtistDetailsDto, ArtistDetailsDto>()
                .ForMember(a => a.ImageUrl,
                    o => o.MapFrom(l => DefaultArtistImage))
                .ForMember(a => a.Biography,
                    o => o.MapFrom(l => l.Biography.Content));
        }
    }
}
