using AutoMapper;
using MusiciansAPP.API.Resources;
using MusiciansAPP.Domain;

namespace MusiciansAPP.API.AutoMapperProfiles
{
    public class ArtistProfile : Profile
    {
        public ArtistProfile()
        {
            CreateMap<Artist, ArtistDto>();
        }
    }
}
