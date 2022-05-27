import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Artist } from '../models/artist';
import { ArtistDetails } from '../models/artist-details';
import { DataService } from './data.service';

@Injectable({
  providedIn: 'root',
})
export class ArtistDetailsService extends DataService<ArtistDetails> {
  constructor(http: HttpClient) {
    super(environment.artistsApiUrl, http);
  }
}
