import { Album } from '../../albums/models/album';
import { Artist } from './artist';
import { Track } from '../../tracks/models/track';

export interface ArtistSupplements {
  topTracks: Track[];
  topAlbums: Album[];
  similarArtists: Artist[];
}
