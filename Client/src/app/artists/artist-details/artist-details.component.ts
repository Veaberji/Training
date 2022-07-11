import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { TabItem } from 'src/app/shared/models/tab-item';
import { Artist } from '../models/artist';
import { ArtistService } from '../services/artist.service';
import { SupplementRoute } from '../supplementRoutes';

@Component({
  selector: 'app-artist-details',
  templateUrl: './artist-details.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ArtistDetailsComponent implements OnInit {
  readonly supplementRoute = SupplementRoute;
  artist$: Observable<Artist> | undefined;

  constructor(private route: ActivatedRoute, private artistService: ArtistService) {}

  ngOnInit(): void {
    this.initArtist();
  }

  private initArtist(): void {
    const name = String(this.route.snapshot.paramMap.get('name'));
    this.artist$ = this.artistService.get(name);
  }
}
