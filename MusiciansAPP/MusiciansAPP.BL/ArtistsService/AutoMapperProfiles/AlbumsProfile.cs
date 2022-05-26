using AutoMapper;
using MusiciansAPP.BL.ArtistsService.Resources;
using MusiciansAPP.Domain;

namespace MusiciansAPP.BL.ArtistsService.AutoMapperProfiles
{
    public class AlbumsProfile : Profile
    {
        public AlbumsProfile()
        {
            CreateMap<AlbumDto, Album>();
        }
    }
}
