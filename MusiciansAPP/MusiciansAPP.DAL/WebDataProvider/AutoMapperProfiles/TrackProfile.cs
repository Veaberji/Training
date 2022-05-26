using AutoMapper;
using MusiciansAPP.BL.ArtistsService.Resources;
using MusiciansAPP.DAL.WebDataProvider.Resources.ArtistTopTracks;

namespace MusiciansAPP.DAL.WebDataProvider.AutoMapperProfiles
{
    public class TrackProfile : Profile
    {
        public TrackProfile()
        {
            CreateMap<LastFmArtistTopTracksDto, ArtistTracksDto>()
                .ForMember(a => a.ArtistName,
                    o => o.MapFrom(l => l.MetaData.ArtistName))
                .ForMember(a => a.Tracks,
                    o => o.MapFrom(l => l.Tracks));
            CreateMap<LastFmArtistTopTrackDto, TrackDto>();
        }
    }
}
