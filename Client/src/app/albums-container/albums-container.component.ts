import { Component, Input, OnInit } from '@angular/core';
import { Album } from '../models/album';

@Component({
  selector: 'app-albums-container',
  templateUrl: './albums-container.component.html',
  styleUrls: ['./albums-container.component.css'],
})
export class AlbumsContainerComponent {
  @Input('albums') albums: Album[] = [];
  @Input('artistName') artistName!: string;
}
