import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { AlbumDetails } from '../models/album-details';
import { AlbumDetailsService } from '../services/album-details.service';

@Component({
  selector: 'app-album-details',
  templateUrl: './album-details.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AlbumDetailsComponent implements OnInit {
  constructor(private route: ActivatedRoute, private service: AlbumDetailsService) {}

  artistName!: string;
  albumTitle!: string;
  album$: Observable<AlbumDetails> | undefined;

  ngOnInit(): void {
    this.initFieldsFromUrl();
    this.initAlbum();
  }

  private initFieldsFromUrl(): void {
    let params = this.route.snapshot.paramMap;
    this.albumTitle = String(params.get('albumTitle'));
    this.artistName = String(params.get('artistName'));
  }

  private initAlbum(): void {
    this.album$ = this.service.getAlbumDetails(this.artistName, this.albumTitle);
  }
}
