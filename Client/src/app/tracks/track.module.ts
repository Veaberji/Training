import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { TopTrackListComponent } from './track-list/track-list.component';
import { AlbumTrackListComponent } from './album-track-list/album-track-list.component';
import { DatePipe } from '@angular/common';

@NgModule({
  declarations: [TopTrackListComponent, AlbumTrackListComponent],
  providers: [DatePipe],
  imports: [SharedModule],
  exports: [TopTrackListComponent, AlbumTrackListComponent],
})
export class TrackModule {}
