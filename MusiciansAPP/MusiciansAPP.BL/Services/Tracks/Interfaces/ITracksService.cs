using MusiciansAPP.BL.Services.Tracks.BLModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusiciansAPP.BL.Services.Tracks.Interfaces;

public interface ITracksService
{
    Task<IEnumerable<TrackBL>> GetArtistTopTracksAsync(string name, int pageSize, int page);
}