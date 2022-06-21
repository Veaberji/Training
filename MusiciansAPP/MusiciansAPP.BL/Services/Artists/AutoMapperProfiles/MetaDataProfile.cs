using AutoMapper;
using MusiciansAPP.BL.Services.Artists.BLModels;
using MusiciansAPP.DAL.DALModels;

namespace MusiciansAPP.BL.Services.Artists.AutoMapperProfiles;

public class MetaDataProfile : Profile
{
    public MetaDataProfile()
    {
        CreateMap<PagingDataDAL, PagingDataBL>();
    }
}