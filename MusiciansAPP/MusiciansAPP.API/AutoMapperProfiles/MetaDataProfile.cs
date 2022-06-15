using AutoMapper;
using MusiciansAPP.API.UIModels;
using MusiciansAPP.BL.Services.Artists.BLModels;

namespace MusiciansAPP.API.AutoMapperProfiles
{
    public class MetaDataProfile : Profile
    {
        public MetaDataProfile()
        {
            CreateMap<PagingDataBL, PagingDataUI>();
        }
    }
}
