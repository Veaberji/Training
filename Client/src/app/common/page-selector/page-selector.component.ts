import { Component, EventEmitter, Input, Output } from '@angular/core';
import PagingData from 'src/app/models/paging-data';
import PagingDetails from 'src/app/models/paging-details';
import { PagingService } from 'src/app/services/paging.service';

@Component({
  selector: 'app-page-selector',
  templateUrl: './page-selector.component.html',
})
export class PageSelectorComponent {
  readonly pagesAmount: number = 10;
  pages: number[] = [];
  firstPage!: number;
  lastPage!: number;
  @Input('pagingData') pagingData!: PagingData;
  @Input('pageSize') pageSize!: number;
  @Input('currentPage') currentPage!: number;
  @Output('currentPageChange') currentPageChange = new EventEmitter<number>();

  constructor(private service: PagingService) {}

  ngOnChanges(): void {
    this.initPages();
  }

  changeCurrentPage(page: number): void {
    this.currentPageChange.emit(page);
  }

  private initPages(): void {
    if (!this.isParamsPassed()) {
      return;
    }

    this.pages = this.service.getPagesArray(this.getPagingDetails());
    this.firstPage = this.pages[0];
    this.lastPage = this.pages[this.pages.length - 1];
  }

  private isParamsPassed(): boolean {
    return this.pageSize > 0 && this.currentPage > 0;
  }

  private getPagingDetails(): PagingDetails {
    const pagingDetails: PagingDetails = {
      CurrentPage: this.currentPage,
      TotalItems: this.pagingData.totalItems,
      PageSize: this.pageSize,
      PagesAmount: this.pagesAmount,
    };

    return pagingDetails;
  }
}
