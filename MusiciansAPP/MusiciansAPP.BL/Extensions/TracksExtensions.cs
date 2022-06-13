using MusiciansAPP.Domain;
using System.Collections.Generic;
using System.Linq;

namespace MusiciansAPP.BL.Extensions;

public static class TracksExtensions
{
    public static bool IsFullData(
        this IEnumerable<Track> tracks, int pageSize)
    {
        return tracks.Count() == pageSize
               && tracks.All(t => t.IsTrackHasPlayCount());
    }
}