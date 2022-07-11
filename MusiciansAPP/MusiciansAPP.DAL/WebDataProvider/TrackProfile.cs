using AutoMapper;
using MusiciansAPP.DAL.DALModels;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.AlbumDetails;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.ArtistTopTracks;

namespace MusiciansAPP.DAL.WebDataProvider;

public class TrackProfile : Profile
{
    public TrackProfile()
    {
        CreateMap<LastFmArtistTopTracksDto, ArtistTracksDAL>()
            .ForMember(
                a => a.ArtistName,
                o => o.MapFrom(l => l.MetaData.ArtistName))
            .ForMember(
                a => a.Tracks,
                o => o.MapFrom(l => l.Tracks));

        CreateMap<LastFmArtistTopTrackDto, TrackDAL>();
        CreateMap<LastFmAlbumTrackDto, TrackDAL>();
    }
}