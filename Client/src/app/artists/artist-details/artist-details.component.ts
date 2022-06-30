import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ArtistDetailsService } from '../services/artist-details.service';
import { ArtistDetails } from '../models/artist-details';
import { SupplementRoute } from '../supplementRoutes';

@Component({
  selector: 'app-artist-details',
  templateUrl: './artist-details.component.html',
})
export class ArtistDetailsComponent implements OnInit {
  constructor(private route: ActivatedRoute, private artistDetailsService: ArtistDetailsService) {}

  private name!: string;
  supplementRoute = SupplementRoute;
  readonly componentRouteName: string = 'details';

  artist: ArtistDetails = {
    name: '',
    imageUrl: '',
    biography: '',
  };

  ngOnInit(): void {
    this.name = String(this.route.snapshot.paramMap.get('name'));
    this.initArtist();
  }

  private initArtist(): void {
    this.artistDetailsService.get(this.name).subscribe((response) => {
      this.artist = response;
    });
  }
}
