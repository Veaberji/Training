﻿using AutoMapper;
using MusiciansAPP.API.UIModels;
using MusiciansAPP.BL.Services.Albums;

namespace MusiciansAPP.API.AutoMapperProfiles;

public class AlbumsProfile : Profile
{
    public AlbumsProfile()
    {
        CreateMap<AlbumBL, AlbumUI>();
    }
}