import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TrackListComponent } from './track-list/track-list.component';
import { AlbumTrackListComponent } from './album-track-list/album-track-list.component';
import { TrackService } from './services/track.service';

@NgModule({
  declarations: [TrackListComponent, AlbumTrackListComponent],
  providers: [TrackService],
  imports: [CommonModule],
  exports: [TrackListComponent, AlbumTrackListComponent],
})
export class TrackModule {}
