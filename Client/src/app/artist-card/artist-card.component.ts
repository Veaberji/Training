import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Artist } from '../models/artist';

@Component({
  selector: 'app-artist-card',
  templateUrl: './artist-card.component.html',
})
export class ArtistCardComponent {
  constructor(private router: Router) {}
  @Input('artist') artist!: Artist;
  @Input('newWindow') newWindow: boolean = false;

  goToArtistPage() {
    const url = `/artist-details/${this.artist.name}`;
    if (this.newWindow) {
      window.open(url, '_blank');
    } else {
      this.router.navigate([url]);
    }
  }
}
