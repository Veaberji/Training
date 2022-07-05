import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { DataService } from '../../shared/services/data.service';
import { ArtistDetails } from '../models/artist-details';

@Injectable({
  providedIn: 'root',
})
export class ArtistDetailsService extends DataService<ArtistDetails> {
  constructor(http: HttpClient) {
    super(environment.artistsApiUrl, http);
  }
}
