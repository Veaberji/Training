import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { TrackModule } from '../tracks/track.module';
import { AlbumDetailsComponent } from './album-details/album-details.component';
import { AlbumsContainerComponent } from './albums-container/albums-container.component';
import { AlbumsCardComponent } from './albums-card/albums-card.component';
import { AlbumService } from './services/album.service';

@NgModule({
  declarations: [AlbumsCardComponent, AlbumDetailsComponent, AlbumsContainerComponent],
  providers: [AlbumService],
  imports: [RouterModule, CommonModule, TrackModule],
  exports: [AlbumDetailsComponent, AlbumsContainerComponent],
})
export class AlbumModule {}
