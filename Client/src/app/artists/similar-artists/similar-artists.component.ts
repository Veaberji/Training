import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { map, Observable } from 'rxjs';
import { ArtistService } from '../services/artist.service';
import { Artist } from '../models/artist';
import { CardItem } from 'src/app/shared/models/card-item';
import { GridSizes } from 'src/app/shared/models/grid-sizes';

@Component({
  selector: 'app-similar-artists',
  templateUrl: './similar-artists.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SimilarArtistsComponent implements OnInit {
  readonly sizes: GridSizes = {
    small: 1,
    middle: 2,
    large: 3,
    extraLarge: 5,
  };
  artists$: Observable<Artist[]> | undefined;

  constructor(private route: ActivatedRoute, private artistService: ArtistService) {}

  ngOnInit(): void {
    this.initSimilarArtists();
  }

  getCardItems(artists: Artist[]): CardItem[] {
    return artists.map((artist) => {
      return {
        name: artist.name,
        imageUrl: artist.imageUrl,
        navUrl: `/artists/details/${artist.name}`,
        newWindow: true,
      };
    });
  }

  private initSimilarArtists(): void {
    const artistName$ = this.route.parent?.params.pipe(map((param) => param['name']));
    if (artistName$) {
      this.artists$ = this.artistService.getSimilarArtists(artistName$);
    }
  }
}
