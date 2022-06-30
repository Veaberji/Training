import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-page-size-selector',
  templateUrl: './page-size-selector.component.html',
})
export class PageSizeSelectorComponent {
  @Input('currentPageSize') currentPageSize!: number;
  @Input('pageSizes') pageSizes!: number[];
  @Output('currentPageSizeChange') currentPageSizeChange = new EventEmitter<number>();

  changeCurrentPageSize(size: number): void {
    this.currentPageSizeChange.emit(size);
  }
}
