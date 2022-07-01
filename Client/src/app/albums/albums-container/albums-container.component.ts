import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { AlbumService } from '../services/album.service';
import { Album } from '../models/album';

@Component({
  selector: 'app-albums-container',
  templateUrl: './albums-container.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TopAlbumsContainerComponent implements OnInit {
  albums$: Observable<Album[]> | undefined;
  artistName!: string;

  constructor(private route: ActivatedRoute, private albumService: AlbumService) {}

  ngOnInit(): void {
    this.initAlbums();
  }

  private initAlbums(): void {
    this.artistName = String(this.route.parent?.snapshot.paramMap.get('name'));
    this.albums$ = this.albumService.getTopAlbums(this.artistName);
  }
}
