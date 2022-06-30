import { Component, Input } from '@angular/core';
import AlbumTrack from '../models/album-track';

@Component({
  selector: 'app-album-track-list',
  templateUrl: './album-track-list.component.html',
})
export class AlbumTrackListComponent {
  @Input('tracks') tracks!: AlbumTrack[];
}
