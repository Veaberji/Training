import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { Observable } from 'rxjs';
import PagingData from '../models/paging-data';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PaginationComponent {
  @Input('pageSizes') pageSizes!: number[];
  @Input('pagingData') pagingData!: PagingData;
  @Input('paging$') paging$: Observable<{ page: number; pageSize: number }> | undefined;
}
