using MusiciansAPP.DAL.DBDataProvider.Interfaces.Repositories;
using MusiciansAPP.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusiciansAPP.DAL.DBDataProvider.Interfaces;

public interface IUnitOfWork
{
    IArtistRepository Artists { get; }
    IAlbumRepository Albums { get; }
    ITrackRepository Tracks { get; }
    Task<int> CompleteAsync();
    Task SaveAlbumDetailsAsync(Album album, IEnumerable<Track> tracks);
}