import AlbumTrack from '../../tracks/models/album-track';

export interface AlbumDetails {
  name: string;
  artistName: string;
  imageUrl: string;
  tracks: AlbumTrack[];
}
