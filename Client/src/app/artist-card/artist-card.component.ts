import { Component, Input, OnInit } from '@angular/core';
import { Artist } from '../models/artist';

@Component({
  selector: 'app-artist-card',
  templateUrl: './artist-card.component.html',
  styleUrls: ['./artist-card.component.css'],
})
export class ArtistCardComponent {
  @Input('artist') artist!: Artist;
}
