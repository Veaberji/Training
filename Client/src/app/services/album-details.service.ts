import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { AlbumDetails } from '../models/album-details';
import { DataService } from './data.service';

@Injectable({
  providedIn: 'root',
})
export class AlbumDetailsService extends DataService<AlbumDetails> {
  constructor(http: HttpClient) {
    super(environment.artistsApiUrl, http);
  }

  getAlbumDetails(artistName: string, albumTitle: string) {
    const albumDetailsApiPostfix = 'album-details';
    return this.get(`${artistName}/${albumDetailsApiPostfix}/${albumTitle}`);
  }
}
