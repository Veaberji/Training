import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TrackService } from '../services/track.service';
import { Track } from '../models/track';

@Component({
  selector: 'app-track-list',
  templateUrl: './track-list.component.html',
})
export class TopTrackListComponent implements OnInit {
  tracks!: Track[];

  constructor(private route: ActivatedRoute, private trackService: TrackService) {}

  ngOnInit(): void {
    this.loadTracks();
  }

  private loadTracks(): void {
    const artistName = String(this.route.parent?.snapshot.paramMap.get('name'));
    this.trackService.getTopTracks(artistName).subscribe((response) => {
      this.tracks = response;
    });
  }
}
