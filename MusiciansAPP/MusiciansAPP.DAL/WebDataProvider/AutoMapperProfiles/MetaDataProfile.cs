using AutoMapper;
using MusiciansAPP.DAL.DALModels;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.Common;

namespace MusiciansAPP.DAL.WebDataProvider.AutoMapperProfiles
{
    public class MetaDataProfile : Profile
    {
        public MetaDataProfile()
        {
            CreateMap<LastFmArtistsMetaDataDto, PagingDataDAL>()
                .ForMember(dest => dest.TotalItems,
                    opt => opt.MapFrom(src => src.Total));
        }
    }
}
