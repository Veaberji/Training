import { Track } from 'src/app/tracks/models/track';

export interface Album {
  name: string;
  imageUrl: string;
  playCount: number;
  artistName: string | null;
  tracks: Track[];
}
