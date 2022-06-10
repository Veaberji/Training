using AutoMapper;
using MusiciansAPP.BL.ArtistsService.BLModels;
using MusiciansAPP.DAL.DALModels;

namespace MusiciansAPP.BL.ArtistsService.AutoMapperProfiles
{
    public class MetaDataProfile : Profile
    {
        public MetaDataProfile()
        {
            CreateMap<PagingDataDAL, PagingDataBL>();
        }
    }
}
