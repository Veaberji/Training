using MusiciansAPP.DAL.DBDataProvider.Interfaces.Repositories;
using System.Threading.Tasks;

namespace MusiciansAPP.DAL.DBDataProvider.Interfaces;

public interface IUnitOfWork
{
    IArtistRepository Artists { get; }
    IAlbumRepository Albums { get; }
    ITrackRepository Tracks { get; }
    Task<int> CompleteAsync();
}