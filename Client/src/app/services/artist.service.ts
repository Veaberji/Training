import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Artist } from '../models/artist';
import { DataService } from './data.service';

@Injectable({
  providedIn: 'root',
})
export class ArtistService extends DataService<Artist> {
  constructor(http: HttpClient) {
    super('https://localhost:7093/api/artists/', http);
  }
}
