import { DatePipe } from '@angular/common';
import { Component, Input } from '@angular/core';
import { TableColumn } from 'src/app/shared/models/table-column';
import AlbumTrack from '../models/album-track';

@Component({
  selector: 'app-album-track-list',
  templateUrl: './album-track-list.component.html',
})
export class AlbumTrackListComponent {
  @Input('tracks') tracks!: AlbumTrack[];

  columns: TableColumn[] = [
    { header: 'Title', field: 'name' },
    { header: 'Duration', field: 'duration' },
  ];

  constructor(private datePipe: DatePipe) {}

  getTracks(): { name: string; duration: string | null }[] {
    let mappedTracks: { name: string; duration: string | null }[] = [];
    this.tracks.forEach((t) => {
      mappedTracks.push({
        name: t.name,
        duration: t.durationInSeconds ? this.datePipe.transform(t.durationInSeconds * 1000, 'mm:ss') : null,
      });
    });

    return mappedTracks;
  }
}
