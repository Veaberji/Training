using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MusiciansAPP.DAL.DALModels;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.AlbumDetails;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.ArtistTopAlbums;

namespace MusiciansAPP.DAL.WebDataProvider;

public class AlbumProfile : Profile
{
    private const string DefaultImageSize = "extralarge";
    private const string DefaultAlbumImage = "https://i.ibb.co/KbYpSBF/default-album.jpg";

    public AlbumProfile()
    {
        CreateMap<LastFmArtistTopAlbumsDto, ArtistAlbumsDAL>()
            .ForMember(
                dest => dest.ArtistName,
                opt => opt.MapFrom(scr => scr.MetaData.ArtistName))
            .ForMember(
                dest => dest.Albums,
                opt => opt.MapFrom(scr => scr.Albums));

        CreateMap<LastFmArtistTopAlbumDto, AlbumDAL>()
            .ForMember(
                dest => dest.ImageUrl,
                opt => opt.MapFrom(scr =>
                    MapImageUrl(scr.Images.First(i => i.Size == DefaultImageSize).Url)));

        CreateMap<LastFmArtistAlbumDto, AlbumDAL>()
            .ForMember(
                dest => dest.ImageUrl,
                opt => opt.MapFrom(scr =>
                        MapImageUrl(scr.Images.First(i => i.Size == DefaultImageSize).Url)))
            .ForMember(
                dest => dest.Tracks,
                opt => opt.MapFrom(scr => scr.Track.Tracks));

        CreateMap<LastFmArtistAlbumOneTrackDto, AlbumDAL>()
            .ForMember(
                dest => dest.ImageUrl,
                opt => opt.MapFrom(scr =>
                    MapImageUrl(scr.Images.First(i => i.Size == DefaultImageSize).Url)))
            .ForMember(
                dest => dest.Tracks,
                opt => opt.MapFrom(scr => new List<LastFmAlbumTrackDto> { scr.Track.Track }));
    }

    private static string MapImageUrl(string imageUrl)
    {
        return string.IsNullOrWhiteSpace(imageUrl) ? DefaultAlbumImage : imageUrl;
    }
}