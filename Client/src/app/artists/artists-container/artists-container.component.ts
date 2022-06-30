import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { ArtistsPagingService } from '../services/artist-paging.service';
import ArtistsPaging from '../models/artist-paging';

@Component({
  selector: 'app-artists-container',
  templateUrl: './artists-container.component.html',
})
export class ArtistsContainerComponent implements OnInit {
  readonly pageSizes: [12, 24, 48] = [12, 24, 48];
  readonly pageParamFromUrl = 'page';
  readonly pageSizeParamFromUrl = 'pageSize';
  artistsPaging: ArtistsPaging = {
    artists: [],
    pagingData: {
      totalItems: 0,
    },
  };
  currentPageSize!: number;
  currentPage!: number;

  constructor(private route: ActivatedRoute, private router: Router, private service: ArtistsPagingService) {}

  ngOnInit(): void {
    this.initUrlParams();
    this.loadArtists();
  }

  changeCurrentPage(page: number): void {
    this.currentPage = page;
    this.loadArtists();
    this.goToNewPage(page, this.currentPageSize);
  }

  changeCurrentPageSize(size: number): void {
    this.currentPageSize = size;
    this.currentPage = 1;
    this.loadArtists();
    this.goToNewPage(this.currentPage, size);
  }

  goToNewPage(page: number, pageSize: number): void {
    const url = `artists/page/${page}/pageSize/${pageSize}`;
    this.router.navigate([url]);
  }

  private loadArtists(): void {
    const pagingQueryString = `?pageSize=${this.currentPageSize}&page=${this.currentPage}`;
    this.service.get(pagingQueryString).subscribe((response) => {
      this.artistsPaging = response;
    });
  }

  private initUrlParams(): void {
    let params = this.route.snapshot.paramMap;
    this.initCurrentPage(params);
    this.initCurrentPageSize(params);
  }

  private initCurrentPage(params: ParamMap): void {
    const page = Number(params.get(this.pageParamFromUrl));
    this.currentPage = !page ? 1 : page;
  }

  private initCurrentPageSize(params: ParamMap): void {
    const pageSize = Number(params.get(this.pageSizeParamFromUrl));
    this.currentPageSize = !pageSize ? this.pageSizes[0] : pageSize;
  }
}
