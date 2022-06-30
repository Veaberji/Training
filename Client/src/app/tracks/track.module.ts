import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TopTrackListComponent } from './track-list/track-list.component';
import { AlbumTrackListComponent } from './album-track-list/album-track-list.component';
import { TrackService } from './services/track.service';

@NgModule({
  declarations: [TopTrackListComponent, AlbumTrackListComponent],
  providers: [TrackService],
  imports: [CommonModule],
  exports: [TopTrackListComponent, AlbumTrackListComponent],
})
export class TrackModule {}
