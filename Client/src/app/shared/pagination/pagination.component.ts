import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PaginationComponent {
  @Input('pageSizes') pageSizes!: number[];
  @Input('totalItems') totalItems!: number;
  @Input('paging$') paging$: Observable<{ page: number; pageSize: number }> | undefined;
}
