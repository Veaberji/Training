import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DataService } from '../../shared/services/data.service';
import { environment } from '../../../environments/environment';
import { Album } from '../models/album';

@Injectable({
  providedIn: 'root',
})
export class AlbumService extends DataService<Album> {
  constructor(http: HttpClient) {
    super(environment.artistsApiUrl, http);
  }

  getTopAlbums(artistName: string): Observable<Album[]> {
    const topAlbumsApiPostfix = 'top-albums';
    return this.getAll(`${artistName}/${topAlbumsApiPostfix}`);
  }

  getAlbumDetails(artistName: string, albumTitle: string): Observable<Album> {
    const albumDetailsApiPostfix = 'album-details';
    return this.get(`${artistName}/${albumDetailsApiPostfix}/${albumTitle}`);
  }
}
