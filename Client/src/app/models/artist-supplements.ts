import { Album } from './album';
import { Artist } from './artist';
import { Track } from './track';

export interface ArtistSupplements {
  topTracks: Track[];
  topAlbums: Album[];
  similarArtists: Artist[];
}
