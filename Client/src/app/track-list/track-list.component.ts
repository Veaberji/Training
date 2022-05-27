import { Component, Input, OnInit } from '@angular/core';
import { Track } from '../models/track';

@Component({
  selector: 'app-track-list',
  templateUrl: './track-list.component.html',
  styleUrls: ['./track-list.component.css'],
})
export class TrackListComponent {
  @Input('tracks') tracks!: Track[];
}
