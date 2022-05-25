import { Component, OnInit } from '@angular/core';
import { Artist } from '../models/artist';
import { ArtistCardComponent } from '../artist-card/artist-card.component';
import { ArtistService } from '../services/artist.service';

@Component({
  selector: 'app-artists-container',
  templateUrl: './artists-container.component.html',
  styleUrls: ['./artists-container.component.css'],
})
export class ArtistsContainerComponent implements OnInit {
  artists: Artist[] = [];
  constructor(private service: ArtistService) {}

  ngOnInit() {
    this.service.getAll().subscribe((response) => {
      this.artists = response;
    });
  }
}
