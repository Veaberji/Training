import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-page-size-selector',
  templateUrl: './page-size-selector.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PageSizeSelectorComponent {
  @Input('pageSizes') pageSizes!: number[];
  @Input('paging$') paging$: Observable<{ page: number; pageSize: number }> | undefined;

  constructor(private router: Router) {}

  onCurrentPageSizeChanged(size: number): void {
    const firstPage = 1;
    const url = `artists/page/${firstPage}/pageSize/${size}`;
    this.router.navigate([url]);
  }
}
