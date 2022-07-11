import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { combineLatest, map, Observable, concatMap, of } from 'rxjs';
import { CardItem } from 'src/app/shared/models/card-item';
import { environment } from 'src/environments/environment';
import { Artist } from '../models/artist';
import { ArtistService } from '../services/artist.service';

@Component({
  selector: 'app-artists-container',
  templateUrl: './artists-container.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ArtistsContainerComponent implements OnInit {
  readonly pageSizes: [12, 24, 48] = [12, 24, 48];
  totalTopArtists: number | undefined;
  topArtists$: Observable<Artist[]> | undefined;
  paging$: Observable<{ page: number; pageSize: number }> | undefined;

  constructor(private route: ActivatedRoute, private service: ArtistService) {}

  ngOnInit(): void {
    this.initTopArtists();
    this.totalTopArtists = environment.totalTopArtists;
  }

  getCardItems(artists: Artist[]): CardItem[] {
    return artists.map((artist) => {
      return {
        name: artist.name,
        imageUrl: artist.imageUrl,
        navUrl: `/artists/details/${artist.name}`,
      };
    });
  }

  private initTopArtists(): void {
    this.initPaging();
    if (!this.paging$) {
      return;
    }

    const pagingQuery$ = this.paging$.pipe(map(({ page, pageSize }) => `?pageSize=${pageSize}&page=${page}`));
    this.topArtists$ = this.service.getAllByObservable(pagingQuery$);
  }

  private initPaging() {
    const params = this.route.params;
    const currentPage$ = params.pipe(map((param) => +param['page']));
    const currentPageSize$ = params.pipe(map((param) => +param['pageSize']));

    this.paging$ = combineLatest([currentPage$, currentPageSize$]).pipe(
      concatMap(([page, pageSize]) => {
        page = !page ? 1 : page;
        pageSize = !pageSize ? this.pageSizes[0] : pageSize;
        return of({ page, pageSize });
      })
    );
  }
}
