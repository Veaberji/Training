import { Component, Input } from '@angular/core';
import { TableColumn } from '../models/table-column';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
})
export class TableComponent {
  @Input('columns') columns: TableColumn[] = [];
  @Input('rows') rows: any[] = [];
}
