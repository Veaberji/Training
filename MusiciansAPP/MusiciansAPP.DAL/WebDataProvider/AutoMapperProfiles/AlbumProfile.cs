using AutoMapper;
using MusiciansAPP.BL.ArtistsService.Resources;
using MusiciansAPP.DAL.WebDataProvider.Resources.ArtistTopAlbums;
using System.Linq;

namespace MusiciansAPP.DAL.WebDataProvider.AutoMapperProfiles
{
    public class AlbumProfile : Profile
    {
        private const string DefaultImageSize = "extralarge";
        public AlbumProfile()
        {
            CreateMap<LastFmArtistTopAlbumsDto, ArtistAlbumsDto>()
                .ForMember(dest => dest.ArtistName,
                    opt => opt.MapFrom(scr => scr.MetaData.ArtistName))
                .ForMember(dest => dest.Albums,
                    opt => opt.MapFrom(scr => scr.Albums));
            CreateMap<LastFmArtistTopAlbumDto, AlbumDto>()
                .ForMember(dest => dest.ImageUrl,
                    opt => opt.MapFrom(scr =>
                        scr.Images.First(i => i.Size == DefaultImageSize).Url));
        }
    }
}
