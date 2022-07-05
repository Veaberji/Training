import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AlbumModule } from '../albums/album.module';
import { SharedModule } from '../shared/shared.module';
import { TrackModule } from '../tracks/track.module';
import { SimilarArtistsComponent } from './similar-artists/similar-artists.component';
import { ArtistDetailsComponent } from './artist-details/artist-details.component';
import { ArtistsContainerComponent } from './artists-container/artists-container.component';

@NgModule({
  declarations: [ArtistsContainerComponent, ArtistDetailsComponent, SimilarArtistsComponent],
  imports: [RouterModule, SharedModule, AlbumModule, TrackModule],
  exports: [ArtistsContainerComponent, ArtistDetailsComponent],
})
export class ArtistModule {}
