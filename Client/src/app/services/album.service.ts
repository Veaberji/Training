import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Album } from '../models/album';
import { DataService } from './data.service';

@Injectable({
  providedIn: 'root',
})
export class AlbumService extends DataService<Album> {
  constructor(http: HttpClient) {
    super(environment.artistsApiUrl, http);
  }

  getTopAlbums(artistName: string) {
    const topAlbumsApiPostfix = 'top-albums';
    return this.getAll(`${artistName}/${topAlbumsApiPostfix}`);
  }
}
