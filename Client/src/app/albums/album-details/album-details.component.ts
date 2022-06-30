import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AlbumDetails } from '../models/album-details';
import { AlbumDetailsService } from '../services/album-details.service';

@Component({
  selector: 'app-album-details',
  templateUrl: './album-details.component.html',
})
export class AlbumDetailsComponent implements OnInit {
  constructor(private route: ActivatedRoute, private service: AlbumDetailsService) {}

  artistName!: string;
  albumTitle!: string;
  album: AlbumDetails = {
    name: '',
    artistName: '',
    imageUrl: '',
    tracks: [],
  };

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
    this.service.getAlbumDetails(this.artistName, this.albumTitle).subscribe((response) => {
      this.album = response;
    });
  }
}
