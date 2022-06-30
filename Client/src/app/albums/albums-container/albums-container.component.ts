import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AlbumService } from '../services/album.service';
import { Album } from '../models/album';

@Component({
  selector: 'app-albums-container',
  templateUrl: './albums-container.component.html',
})
export class TopAlbumsContainerComponent implements OnInit {
  albums: Album[] = [];
  artistName!: string;

  constructor(private route: ActivatedRoute, private albumService: AlbumService) {}

  ngOnInit(): void {
    this.loadAlbums();
  }

  private loadAlbums(): void {
    this.artistName = String(this.route.parent?.snapshot.paramMap.get('name'));
    this.albumService.getTopAlbums(this.artistName).subscribe((response) => {
      this.albums = response;
    });
  }
}
