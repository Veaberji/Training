import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { combineLatest, map, Observable, concatMap, of } from 'rxjs';
import { ArtistsPagingService } from '../services/artist-paging.service';
import ArtistsPaging from '../models/artist-paging';

@Component({
  selector: 'app-artists-container',
  templateUrl: './artists-container.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ArtistsContainerComponent implements OnInit {
  readonly pageSizes: [12, 24, 48] = [12, 24, 48];
  readonly pageParamFromUrl = 'page';
  readonly pageSizeParamFromUrl = 'pageSize';
  artistsPaging$: Observable<ArtistsPaging> | undefined;
  paging$: Observable<{ page: number; pageSize: number }> | undefined;

  constructor(private route: ActivatedRoute, private service: ArtistsPagingService) {}

  ngOnInit(): void {
    this.initArtistsPaging();
  }

  private initArtistsPaging(): void {
    this.initPaging();
    if (!this.paging$) {
      return;
    }

    const pagingQuery$ = this.paging$.pipe(map(({ page, pageSize }) => `?pageSize=${pageSize}&page=${page}`));
    this.artistsPaging$ = this.service.getByObservable(pagingQuery$);
  }

  private initPaging() {
    const params = this.route.params;
    const currentPage$ = params.pipe(map((param) => +param[this.pageParamFromUrl]));
    const currentPageSize$ = params.pipe(map((param) => +param[this.pageSizeParamFromUrl]));

    this.paging$ = combineLatest([currentPage$, currentPageSize$]).pipe(
      concatMap(([page, pageSize]) => {
        page = !page ? 1 : page;
        pageSize = !pageSize ? this.pageSizes[0] : pageSize;
        return of({ page, pageSize });
      })
    );
  }
}
