using AutoMapper;
using MusiciansAPP.DAL.DALModels;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.AlbumDetails;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.ArtistTopAlbums;
using System.Collections.Generic;
using System.Linq;

namespace MusiciansAPP.DAL.WebDataProvider.AutoMapperProfiles;

public class AlbumProfile : Profile
{
    private const string DefaultImageSize = "extralarge";
    private const string DefaultAlbumImage = "https://i.ibb.co/KbYpSBF/default-album.jpg";

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
                    MapImageUrl(scr.Images.First(i => i.Size == DefaultImageSize).Url)));

        CreateMap<LastFmArtistAlbumDto, AlbumDetailsDAL>()
            .ForMember(dest => dest.ImageUrl,
                opt => opt.MapFrom(scr =>
                        MapImageUrl(scr.Images.First(i => i.Size == DefaultImageSize).Url)))
            .ForMember(dest => dest.Tracks,
                opt => opt.MapFrom(scr => scr.Track.Tracks));

        CreateMap<LastFmArtistAlbumOneTrackDto, AlbumDetailsDAL>()
            .ForMember(dest => dest.ImageUrl,
                opt => opt.MapFrom(scr =>
                    MapImageUrl(scr.Images.First(i => i.Size == DefaultImageSize).Url)))
            .ForMember(dest => dest.Tracks,
        opt => opt.MapFrom(scr => new List<LastFmAlbumTrackDto> { scr.Track.Track }));
    }

    private string MapImageUrl(string imageUrl)
    {
        return string.IsNullOrWhiteSpace(imageUrl) ? DefaultAlbumImage : imageUrl;
    }
}