import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { map, Observable } from 'rxjs';
import { ArtistService } from '../services/artist.service';
import { Artist } from '../models/artist';

@Component({
  selector: 'app-similar-artists',
  templateUrl: './similar-artists.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SimilarArtistsComponent implements OnInit {
  artists$: Observable<Artist[]> | undefined;

  constructor(private route: ActivatedRoute, private artistService: ArtistService) {}

  ngOnInit(): void {
    this.initSimilarArtists();
  }

  private initSimilarArtists(): void {
    const artistName$ = this.route.parent?.params.pipe(map((param) => param['name']));
    if (artistName$) {
      this.artists$ = this.artistService.getSimilarArtists(artistName$);
    }
  }
}
