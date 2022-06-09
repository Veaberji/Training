using MusiciansAPP.DAL.DBDataProvider.Interfaces;
using MusiciansAPP.DAL.DBDataProvider.Interfaces.Repositories;
using MusiciansAPP.DAL.DBDataProvider.Logic.Repositories;
using MusiciansAPP.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusiciansAPP.DAL.DBDataProvider.Logic;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Artists = new ArtistRepository(_context);
        Albums = new AlbumRepository(_context);
        Tracks = new TrackRepository(_context);
    }

    public IArtistRepository Artists { get; }
    public IAlbumRepository Albums { get; }
    public ITrackRepository Tracks { get; }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task SaveAlbumDetailsAsync(Album album, IEnumerable<Track> tracks)
    {
        var addedAlbum = await Albums.AddOrUpdateAlbumDetailsAsync(album);
        await UpdateAlbumTracksAsync(addedAlbum, tracks);
    }

    private async Task UpdateAlbumTracksAsync(Album album,
        IEnumerable<Track> tracks)
    {
        await Tracks.AddOrUpdateAlbumTracksAsync(album, tracks);
        await CompleteAsync();
    }
}