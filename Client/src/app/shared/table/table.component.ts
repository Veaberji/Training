import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { TableColumn } from '../models/table-column';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TableComponent implements OnInit {
  @Input('columns') columns: TableColumn[] = [];
  @Input('rows') rows: any[] = [];

  displayedColumns: Array<string> = [];
  dataSource: MatTableDataSource<any> = new MatTableDataSource();

  ngOnInit(): void {
    this.displayedColumns = this.columns.map((c) => c.header);
    this.dataSource = new MatTableDataSource(this.rows);
  }
}
