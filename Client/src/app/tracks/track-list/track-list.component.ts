import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { map, Observable } from 'rxjs';
import { TrackService } from '../services/track.service';
import { Track } from '../models/track';
import { TableColumn } from 'src/app/shared/models/table-column';

@Component({
  selector: 'app-track-list',
  templateUrl: './track-list.component.html',
})
export class TopTrackListComponent implements OnInit {
  tracks$: Observable<Track[]> | undefined;

  constructor(private route: ActivatedRoute, private trackService: TrackService) {}

  ngOnInit(): void {
    this.loadTracks();
  }

  columns: TableColumn[] = [
    { header: '#', field: '' },
    { header: 'Title', field: 'name' },
    { header: 'Times Playied', field: 'playCount' },
  ];

  private loadTracks(): void {
    const artistName = String(this.route.parent?.snapshot.paramMap.get('name'));
    this.tracks$ = this.trackService
      .getTopTracks(artistName)
      .pipe(map((tracks) => tracks.sort((t1, t2) => t2.playCount - t1.playCount)));
  }
}
