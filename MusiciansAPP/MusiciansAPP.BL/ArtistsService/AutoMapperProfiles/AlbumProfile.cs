﻿using AutoMapper;
using MusiciansAPP.BL.ArtistsService.BLModels;
using MusiciansAPP.DAL.DALModels;
using MusiciansAPP.Domain;

namespace MusiciansAPP.BL.ArtistsService.AutoMapperProfiles;

public class AlbumProfile : Profile
{
    public AlbumProfile()
    {
        CreateMap<AlbumDAL, AlbumBL>();
        CreateMap<AlbumBL, Album>();

        CreateMap<AlbumDetailsDAL, AlbumDetailsBL>();
        CreateMap<AlbumDetailsBL, Album>();

        CreateMap<ArtistAlbumsDAL, ArtistAlbumsBL>();
    }
}