import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AlbumModule } from '../albums/album.module';
import { SharedModule } from '../shared/shared.module';
import { TrackModule } from '../tracks/track.module';
import { SimilarArtistsComponent } from './similar-artists/similar-artists.component';
import { ArtistCardComponent } from './artist-card/artist-card.component';
import { ArtistDetailsComponent } from './artist-details/artist-details.component';
import { ArtistsContainerComponent } from './artists-container/artists-container.component';
import { ArtistDetailsService } from './services/artist-details.service';
import { ArtistsPagingService } from './services/artist-paging.service';
import { ArtistService } from './services/artist.service';

@NgModule({
  declarations: [ArtistCardComponent, ArtistsContainerComponent, ArtistDetailsComponent, SimilarArtistsComponent],
  providers: [ArtistService, ArtistsPagingService, ArtistDetailsService],
  imports: [RouterModule, SharedModule, AlbumModule, TrackModule],
  exports: [ArtistCardComponent, ArtistsContainerComponent, ArtistDetailsComponent],
})
export class ArtistModule {}