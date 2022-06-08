using AutoMapper;
using MusiciansAPP.DAL.DBDataProvider.Interfaces;
using MusiciansAPP.DAL.DBDataProvider.Interfaces.Repositories;
using MusiciansAPP.DAL.DBDataProvider.Logic.Repositories;
using System.Threading.Tasks;

namespace MusiciansAPP.DAL.DBDataProvider.Logic;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context, IMapper mapper)
    {
        _context = context;
        Artists = new ArtistRepository(_context, mapper);
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
}