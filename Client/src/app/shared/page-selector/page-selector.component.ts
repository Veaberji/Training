import { ChangeDetectionStrategy, Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { PagingService } from '../services/paging.service';
import PagingData from '../models/paging-data';
import PagingDetails from '../models/paging-details';

@Component({
  selector: 'app-page-selector',
  templateUrl: './page-selector.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PageSelectorComponent implements OnInit, OnDestroy {
  private _subscribtion: Subscription | undefined;
  readonly pagesAmount: number = 10;
  pages: number[] = [];
  firstPage!: number;
  lastPage!: number;
  @Input('pagingData') pagingData!: PagingData;
  @Input('paging$') paging$: Observable<{ page: number; pageSize: number }> | undefined;

  constructor(private service: PagingService) {}

  ngOnInit(): void {
    this.initPages();
  }

  ngOnDestroy(): void {
    this._subscribtion?.unsubscribe();
  }

  private initPages(): void {
    this._subscribtion = this.paging$?.subscribe(({ page, pageSize }) => {
      this.pages = this.service.getPagesArray(this.getPagingDetails(page, pageSize));
      this.firstPage = this.pages[0];
      this.lastPage = this.pages[this.pages.length - 1];
    });
  }

  private getPagingDetails(page: number, pageSize: number): PagingDetails {
    const pagingDetails: PagingDetails = {
      CurrentPage: page,
      TotalItems: this.pagingData.totalItems,
      PageSize: pageSize,
      PagesAmount: this.pagesAmount,
    };

    return pagingDetails;
  }
}
