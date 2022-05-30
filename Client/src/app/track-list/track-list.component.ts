import { Component, Input } from '@angular/core';
import { Track } from '../models/track';

@Component({
  selector: 'app-track-list',
  templateUrl: './track-list.component.html',
})
export class TrackListComponent {
  @Input('tracks') tracks!: Track[];
}
