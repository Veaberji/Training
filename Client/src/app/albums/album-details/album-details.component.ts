import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Album } from '../models/album';

@Component({
  selector: 'app-album-details',
  templateUrl: './album-details.component.html',
  styleUrls: ['./album-details.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AlbumDetailsComponent implements OnInit {
  album: Album | undefined;

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.album = this.route.snapshot.data['resolvedData'];
  }
}
