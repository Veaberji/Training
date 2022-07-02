import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AlbumDetails } from '../models/album-details';

@Component({
  selector: 'app-album-details',
  templateUrl: './album-details.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AlbumDetailsComponent implements OnInit {
  album: AlbumDetails | undefined;

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.album = this.route.snapshot.data['resolvedData'];
  }
}
