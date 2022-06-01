using MusiciansAPP.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusiciansAPP.DAL.DBDataProvider.Interfaces.Repositories;

public interface IArtistRepository : IRepository<Artist>
{
    Task<IEnumerable<Artist>> GetTopArtistsAsync(int pageSize, int page);
    Task<Artist> GetArtistDetailsAsync(string artistName);
    Task<Artist> GetArtistWithSimilarAsync(string artistName, int pageSize, int page);
}