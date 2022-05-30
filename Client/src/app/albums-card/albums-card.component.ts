import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Album } from '../models/album';

@Component({
  selector: 'app-albums-card',
  templateUrl: './albums-card.component.html',
})
export class AlbumsCardComponent implements OnInit {
  constructor(private route: ActivatedRoute) {}
  @Input('album') album!: Album;
  @Input('artistName') artistName!: string;

  ngOnInit(): void {
    let params = this.route.snapshot.paramMap;
  }
}
