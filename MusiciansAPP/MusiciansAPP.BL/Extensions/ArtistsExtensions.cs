using MusiciansAPP.Domain;
using System.Collections.Generic;
using System.Linq;

namespace MusiciansAPP.BL.Extensions;

public static class ArtistsExtensions
{
    public static bool IsFullData(
        this IEnumerable<Artist> artists, int pageSize)
    {
        return artists.Count() == pageSize
               && artists.All(a => a.IsArtistHasImageUrl());
    }
}