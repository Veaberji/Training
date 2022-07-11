﻿using AutoMapper;
using MusiciansAPP.DAL.DALModels;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.ArtistDetails;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.SimilarArtists;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.TopArtists;

namespace MusiciansAPP.DAL.WebDataProvider;

public class ArtistProfile : Profile
{
    private const string DefaultArtistImage = "https://i.ibb.co/6H89Zzh/default-artist.jpg";

    public ArtistProfile()
    {
        CreateMap<LastFmArtistDto, ArtistDAL>()
            .ForMember(
                dest => dest.ImageUrl,
                opt => opt.MapFrom(src => DefaultArtistImage));

        CreateMap<LastFmArtistDetailsDto, ArtistDAL>()
            .ForMember(
                dest => dest.ImageUrl,
                opt => opt.MapFrom(src => DefaultArtistImage))
            .ForMember(
                dest => dest.Biography,
                o => o.MapFrom(src => src.Biography.Content));

        CreateMap<LastFmSimilarArtistDto, ArtistDAL>()
            .ForMember(
                dest => dest.ImageUrl,
                opt => opt.MapFrom(src => DefaultArtistImage));

        CreateMap<LastFmSimilarArtistsDto, SimilarArtistsDAL>()
            .ForMember(
                dest => dest.ArtistName,
                opt => opt.MapFrom(src => src.MetaData.ArtistName))
            .ForMember(
                dest => dest.Artists,
                opt => opt.MapFrom(src => src.Artists));
    }
}