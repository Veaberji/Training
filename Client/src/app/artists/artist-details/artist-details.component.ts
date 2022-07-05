import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { ArtistDetailsService } from '../services/artist-details.service';
import { ArtistDetails } from '../models/artist-details';
import { SupplementRoute } from '../supplementRoutes';

@Component({
  selector: 'app-artist-details',
  templateUrl: './artist-details.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ArtistDetailsComponent implements OnInit {
  readonly componentRouteName: string = 'details';
  supplementRoute = SupplementRoute;
  artist$: Observable<ArtistDetails> | undefined;

  constructor(private route: ActivatedRoute, private artistDetailsService: ArtistDetailsService) {}

  ngOnInit(): void {
    this.initArtist();
  }

  private initArtist(): void {
    const name = String(this.route.snapshot.paramMap.get('name'));
    this.artist$ = this.artistDetailsService.get(name);
  }
}
