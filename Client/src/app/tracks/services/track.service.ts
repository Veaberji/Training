import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { DataService } from '../../shared/services/data.service';
import { Track } from '../models/track';

@Injectable({
  providedIn: 'root',
})
export class TrackService extends DataService<Track> {
  constructor(http: HttpClient) {
    super(environment.artistsApiUrl, http);
  }

  getTopTracks(artistName: string): Observable<Track[]> {
    const topTracksApiPostfix = 'top-tracks';
    return this.getAll(`${artistName}/${topTracksApiPostfix}`);
  }
}
