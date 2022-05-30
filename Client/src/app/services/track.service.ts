import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Track } from '../models/track';
import { DataService } from './data.service';

@Injectable({
  providedIn: 'root',
})
export class TrackService extends DataService<Track> {
  constructor(http: HttpClient) {
    super(environment.artistsApiUrl, http);
  }

  getTopTracks(artistName: string) {
    const topTracksApiPostfix = 'top-tracks';
    return this.getAll(`${artistName}/${topTracksApiPostfix}`);
  }
}
