using AutoMapper;
using MusiciansAPP.DAL.DALModels;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.ArtistTopAlbums;
using System.Linq;

namespace MusiciansAPP.DAL.WebDataProvider.AutoMapperProfiles;

public class AlbumProfile : Profile
{
    private const string DefaultImageSize = "extralarge";

    public AlbumProfile()
    {
        CreateMap<LastFmArtistTopAlbumsDto, ArtistAlbumsDAL>()
            .ForMember(dest => dest.ArtistName,
                opt => opt.MapFrom(scr => scr.MetaData.ArtistName))
            .ForMember(dest => dest.Albums,
                opt => opt.MapFrom(scr => scr.Albums));

        CreateMap<LastFmArtistTopAlbumDto, AlbumDAL>()
            .ForMember(dest => dest.ImageUrl,
                opt => opt.MapFrom(scr =>
                    scr.Images.First(i => i.Size == DefaultImageSize).Url));
    }
}