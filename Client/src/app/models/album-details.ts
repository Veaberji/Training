import AlbumTrack from './album-track';

export interface AlbumDetails {
  name: string;
  artistName: string;
  imageUrl: string;
  tracks: AlbumTrack[];
}
