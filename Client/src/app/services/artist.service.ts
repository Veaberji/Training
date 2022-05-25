import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Artist } from '../models/artist';
import { DataService } from './data.service';

@Injectable({
  providedIn: 'root',
})
export class ArtistService extends DataService<Artist> {
  constructor(http: HttpClient) {
    super(environment.artistsApiUrl, http);
  }
}
