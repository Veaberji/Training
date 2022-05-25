using AutoMapper;
using MusiciansAPP.DAL.WebDataProvider.Resources;
using MusiciansAPP.Domain;
using System;

namespace MusiciansAPP.DAL.WebDataProvider.AutoMapperProfiles
{
    public class ArtistProfile : Profile
    {
        private const string DefaultArtistImage = "https://i.ibb.co/6H89Zzh/default-artist.jpg";
        public ArtistProfile()
        {
            CreateMap<LastFmArtistDto, Artist>()
                .ForMember(a => a.Id,
                    o => o.MapFrom(l => Guid.NewGuid()))
                .ForMember(a => a.ImageUrl,
                    o => o.MapFrom(l => DefaultArtistImage));
        }
    }
}
