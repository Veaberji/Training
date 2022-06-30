import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ArtistService } from '../services/artist.service';
import { Artist } from '../models/artist';

@Component({
  selector: 'app-similar-artists',
  templateUrl: './similar-artists.component.html',
})
export class SimilarArtistsComponent implements OnInit {
  artists: Artist[] = [];

  constructor(private route: ActivatedRoute, private artistService: ArtistService) {}

  ngOnInit(): void {
    this.loadSimilarArtists();
  }

  private loadSimilarArtists(): void {
    const artistName = String(this.route.parent?.snapshot.paramMap.get('name'));
    this.artistService.getSimilarArtists(artistName).subscribe((response) => {
      this.artists = response;
    });
  }
}
