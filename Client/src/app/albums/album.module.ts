import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { TrackModule } from '../tracks/track.module';
import { AlbumDetailsComponent } from './album-details/album-details.component';
import { TopAlbumsContainerComponent } from './albums-container/albums-container.component';
import { AlbumsCardComponent } from './albums-card/albums-card.component';
import { AlbumService } from './services/album.service';

@NgModule({
  declarations: [AlbumsCardComponent, AlbumDetailsComponent, TopAlbumsContainerComponent],
  providers: [AlbumService],
  imports: [RouterModule, CommonModule, TrackModule],
  exports: [AlbumDetailsComponent, TopAlbumsContainerComponent],
})
export class AlbumModule {}
