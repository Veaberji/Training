import { Component, Input, OnInit } from '@angular/core';
import { Artist } from '../models/artist';

@Component({
  selector: 'app-similar-artists',
  templateUrl: './similar-artists.component.html',
})
export class SimilarArtistsComponent {
  @Input('artists') artists: Artist[] = [];
}
