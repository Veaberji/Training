import { Component, EventEmitter, Input, Output } from '@angular/core';
import PagingData from 'src/app/models/paging-data';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
})
export class PaginationComponent {
  @Input('pageSizes') pageSizes!: number[];
  @Input('currentPage') currentPage!: number;
  @Input('currentPageSize') currentPageSize!: number;
  @Input('pagingData') pagingData!: PagingData;
  @Output('currentPageChange') currentPageChange = new EventEmitter<number>();
  @Output('currentPageSizeChanged') currentPageSizeChanged = new EventEmitter<number>();

  onChangeCurrentPageChanged(page: number): void {
    this.currentPageChange.emit(page);
  }

  onCurrentPageSizeChanged(size: number): void {
    this.currentPageSizeChanged.emit(size);
  }
}
