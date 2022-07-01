import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { Album } from '../models/album';

@Component({
  selector: 'app-albums-card',
  templateUrl: './albums-card.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AlbumsCardComponent {
  @Input('album') album!: Album;
  @Input('artistName') artistName!: string;
}
