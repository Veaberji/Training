﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusiciansAPP.Domain;

namespace MusiciansAPP.DAL.DBDataProvider;

public class AlbumRepository : Repository<Album>, IAlbumRepository
{
    public AlbumRepository(DbContext context)
        : base(context)
    {
    }

    private DbSet<Album> Albums => (Context as AppDbContext)?.Albums;

    public async Task<Album> GetAlbumDetailsAsync(string artistName, string albumName)
    {
        return await Albums.Where(
                a => a.Artist.Name == artistName && a.Name == albumName)
            .Include(a => a.Artist)
            .Include(a => a.Tracks)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Album>> GetTopAlbumsForArtistAsync(
        string artistName, int pageSize, int page)
    {
        return await Albums.Where(
                a => a.Artist.Name == artistName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task AddOrUpdateArtistAlbumsAsync(
        Artist artist, IEnumerable<Album> albums)
    {
        var albumsFromDb = await GetAlbumsFromDbAsync(artist.Name, albums);
        var newAlbums = CreateNotExistingAlbums(artist, albums, albumsFromDb);
        UpdateAlbumsPlayCount(albums, albumsFromDb);
        UpdateAlbumsImageUrl(albums, albumsFromDb);
        await AddRangeAsync(newAlbums);
    }

    public async Task<Album> AddOrUpdateAlbumDetailsAsync(Album album)
    {
        var albumFromDb = await GetAlbumDetailsAsync(album.Artist.Name, album.Name);
        if (albumFromDb is null)
        {
            await AddAsync(album);
            return album;
        }

        if (albumFromDb.IsAlbumHasImageUrl())
        {
            return albumFromDb;
        }

        UpdateAlbumImageUrl(albumFromDb, album.ImageUrl);

        return albumFromDb;
    }

    private static void AddArtistToAlbums(
        Artist artist, IEnumerable<Album> newAlbums)
    {
        foreach (var album in newAlbums)
        {
            album.Artist = artist;
        }
    }

    private static void UpdateAlbumsPlayCount(
        IEnumerable<Album> albums, IEnumerable<Album> albumsFromDb)
    {
        foreach (var albumFromDb in albumsFromDb.Where(a => !a.IsAlbumHasPlayCount()))
        {
            var album = albums.First(a => string
                .Equals(a.Name, albumFromDb.Name, StringComparison.OrdinalIgnoreCase));
            albumFromDb.PlayCount = album.PlayCount;
        }
    }

    private static void UpdateAlbumsImageUrl(
        IEnumerable<Album> albums, IEnumerable<Album> albumsFromDb)
    {
        foreach (var albumFromDb in albumsFromDb.Where(a => !a.IsAlbumHasImageUrl()))
        {
            var album = albums.First(a => a.Name == albumFromDb.Name);
            albumFromDb.ImageUrl = album.ImageUrl;
        }
    }

    private static void UpdateAlbumImageUrl(Album album, string imageUrl)
    {
        album.ImageUrl = imageUrl;
    }

    private async Task<IEnumerable<Album>> GetAlbumsFromDbAsync(
        string artistName, IEnumerable<Album> albums)
    {
        var albumsNames = albums.Select(a => a.Name).ToList();
        return await FindAsync(a => albumsNames.Contains(a.Name)
                            && a.Artist.Name == artistName);
    }

    private IEnumerable<Album> CreateNotExistingAlbums(
        Artist artist,
        IEnumerable<Album> albums,
        IEnumerable<Album> albumsFromDb)
    {
        var newAlbums = GetNewAlbums(albums, albumsFromDb);
        AddArtistToAlbums(artist, newAlbums);

        return newAlbums;
    }

    private IEnumerable<Album> GetNewAlbums(
        IEnumerable<Album> albums, IEnumerable<Album> albumsFromDb)
    {
        var albumsFromDbNames = albumsFromDb.Select(a => a.Name).ToList();
        return albums
            .Where(album => IsNewItem(albumsFromDbNames, album.Name))
            .ToList();
    }
}