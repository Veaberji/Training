import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { DataService } from './data.service';
import ArtistsPaging from '../models/artist-paging';

@Injectable({
  providedIn: 'root',
})
export class ArtistsPagingService extends DataService<ArtistsPaging> {
  constructor(http: HttpClient) {
    super(environment.artistsApiUrl, http);
  }
}
