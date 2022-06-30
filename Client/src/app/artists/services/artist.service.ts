import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Artist } from '../models/artist';
import { DataService } from '../../shared/services/data.service';

@Injectable({
  providedIn: 'root',
})
export class ArtistService extends DataService<Artist> {
  constructor(http: HttpClient) {
    super(environment.artistsApiUrl, http);
  }

  getSimilarArtists(artistName: string) {
    const similarArtistsApiPostfix = 'similar';
    return this.getAll(`${artistName}/${similarArtistsApiPostfix}`);
  }
}
