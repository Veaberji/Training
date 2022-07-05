import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { DataService } from '../../shared/services/data.service';
import { Artist } from '../models/artist';

@Injectable({
  providedIn: 'root',
})
export class ArtistService extends DataService<Artist> {
  constructor(http: HttpClient) {
    super(environment.artistsApiUrl, http);
  }

  getSimilarArtists(artistName$: Observable<string>): Observable<Artist[]> {
    const similarArtistsApiPostfix = 'similar';
    const query$: Observable<string> = artistName$.pipe(map((name) => `${name}/${similarArtistsApiPostfix}`));
    return this.getAllByObservable(query$);
  }
}
