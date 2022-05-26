using AutoMapper;
using MusiciansAPP.BL.ArtistsService.Resources;
using MusiciansAPP.DAL.WebDataProvider.Resources.ArtistTopAlbums;

namespace MusiciansAPP.DAL.WebDataProvider.AutoMapperProfiles
{
    public class AlbumProfile : Profile
    {
        public AlbumProfile()
        {
            CreateMap<LastFmArtistTopAlbumsDto, ArtistAlbumsDto>()
                .ForMember(a => a.ArtistName,
                    o => o.MapFrom(l => l.MetaData.ArtistName))
                .ForMember(a => a.Albums,
                    o => o.MapFrom(l => l.Albums));
            CreateMap<LastFmArtistTopAlbumDto, AlbumDto>();
        }
    }
}
