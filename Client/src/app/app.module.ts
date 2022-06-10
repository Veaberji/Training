import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DataService } from './services/data.service';
import { ArtistService } from './services/artist.service';
import { ArtistCardComponent } from './artist-card/artist-card.component';
import { ArtistsContainerComponent } from './artists-container/artists-container.component';
import { ArtistDetailsComponent } from './artist-details/artist-details.component';
import { ArtistDetailsService } from './services/artist-details.service';
import { TrackService } from './services/track.service';
import { AlbumService } from './services/album.service';
import { TrackListComponent } from './track-list/track-list.component';
import { AlbumsContainerComponent } from './albums-container/albums-container.component';
import { AlbumsCardComponent } from './albums-card/albums-card.component';
import { AlbumDetailsComponent } from './album-details/album-details.component';
import { SimilarArtistsComponent } from './similar-artists/similar-artists.component';
import { NotFoundComponent } from './common/not-found/not-found.component';
import { AlbumTrackListComponent } from './album-track-list/album-track-list.component';
import { NavBarComponent } from './common/nav-bar/nav-bar.component';
import { FooterComponent } from './common/footer/footer.component';
import { PaginationComponent } from './common/pagination/pagination.component';
import { PageSizeSelectorComponent } from './common/page-size-selector/page-size-selector.component';
import { PageSelectorComponent } from './common/page-selector/page-selector.component';
import { PagingService } from './services/paging.service';
import { ArtistsPagingService } from './services/artist-paging.service';

@NgModule({
  declarations: [
    AppComponent,
    ArtistCardComponent,
    ArtistsContainerComponent,
    ArtistDetailsComponent,
    TrackListComponent,
    AlbumsContainerComponent,
    AlbumsCardComponent,
    AlbumDetailsComponent,
    SimilarArtistsComponent,
    NotFoundComponent,
    AlbumTrackListComponent,
    NavBarComponent,
    FooterComponent,
    PaginationComponent,
    PageSizeSelectorComponent,
    PageSelectorComponent,
  ],
  imports: [BrowserModule, AppRoutingModule, HttpClientModule],
  providers: [
    DataService,
    ArtistService,
    ArtistsPagingService,
    ArtistDetailsService,
    TrackService,
    AlbumService,
    PagingService,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
