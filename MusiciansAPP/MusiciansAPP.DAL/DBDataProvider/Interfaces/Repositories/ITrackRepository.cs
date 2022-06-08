using MusiciansAPP.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusiciansAPP.DAL.DBDataProvider.Interfaces.Repositories;

public interface ITrackRepository : IRepository<Track>
{
    Task<IEnumerable<Track>> GetTopTracksForArtistAsync(
        string artistName, int pageSize, int page);
    Task AddOrUpdateArtistTracksAsync(Artist artist, IEnumerable<Track> tracks);
    Task AddOrUpdateAlbumTracksAsync(Album album);

}