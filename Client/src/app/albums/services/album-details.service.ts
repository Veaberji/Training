import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AlbumDetails } from '../models/album-details';
import { DataService } from '../../shared/services/data.service';

@Injectable({
  providedIn: 'root',
})
export class AlbumDetailsService extends DataService<AlbumDetails> {
  constructor(http: HttpClient) {
    super(environment.artistsApiUrl, http);
  }

  getAlbumDetails(artistName: string, albumTitle: string): Observable<AlbumDetails> {
    const albumDetailsApiPostfix = 'album-details';
    return this.get(`${artistName}/${albumDetailsApiPostfix}/${albumTitle}`);
  }
}
