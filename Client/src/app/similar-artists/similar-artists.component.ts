import { Component, Input, OnInit } from '@angular/core';
import { Artist } from '../models/artist';

@Component({
  selector: 'app-similar-artists',
  templateUrl: './similar-artists.component.html',
  styleUrls: ['./similar-artists.component.css'],
})
export class SimilarArtistsComponent {
  @Input('artists') artists: Artist[] = [];
}
