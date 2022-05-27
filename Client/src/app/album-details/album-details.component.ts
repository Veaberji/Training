import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-album-details',
  templateUrl: './album-details.component.html',
  styleUrls: ['./album-details.component.css'],
})
export class AlbumDetailsComponent implements OnInit {
  artistName!: string;
  albumTitle!: string;

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    let params = this.route.snapshot.paramMap;
    this.albumTitle = String(params.get('albumTitle'));
    this.artistName = String(params.get('artistName'));
  }
}
