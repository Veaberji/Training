import { Component, Input } from '@angular/core';
import { TabItem } from '../models/tab-item';

@Component({
  selector: 'app-tab',
  templateUrl: './tab.component.html',
  styleUrls: ['./tab.component.css'],
})
export class TabComponent {
  @Input('tabs') tabs: TabItem[] = [];
}
