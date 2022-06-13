using MusiciansAPP.Domain;
using System.Collections.Generic;
using System.Linq;

namespace MusiciansAPP.BL.Extensions;

public static class AlbumsExtensions
{
    public static bool IsFullData(
        this IEnumerable<Album> albums, int pageSize)
    {
        return albums.Count() == pageSize
               && albums.All(a => a.IsAlbumHasImageUrl() && a.IsAlbumHasPlayCount());
    }
}