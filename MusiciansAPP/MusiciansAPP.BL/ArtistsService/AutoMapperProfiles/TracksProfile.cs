using AutoMapper;
using MusiciansAPP.BL.ArtistsService.Resources;
using MusiciansAPP.Domain;

namespace MusiciansAPP.BL.ArtistsService.AutoMapperProfiles
{
    public class TracksProfile : Profile
    {
        public TracksProfile()
        {
            CreateMap<TrackDto, Track>();
        }
    }
}
