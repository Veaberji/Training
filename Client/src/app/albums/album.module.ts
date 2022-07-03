import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { TrackModule } from '../tracks/track.module';
import { AlbumDetailsComponent } from './album-details/album-details.component';
import { TopAlbumsContainerComponent } from './albums-container/albums-container.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [AlbumDetailsComponent, TopAlbumsContainerComponent],
  imports: [RouterModule, SharedModule, TrackModule],
  exports: [AlbumDetailsComponent, TopAlbumsContainerComponent],
})
export class AlbumModule {}
