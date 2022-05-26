import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DataService } from './services/data.service';
import { ArtistService } from './services/artist.service';
import { ArtistCardComponent } from './artist-card/artist-card.component';
import { ArtistsContainerComponent } from './artists-container/artists-container.component';
import { RouterModule, Routes } from '@angular/router';
import { ArtistdetailsComponent } from './artist-details/artist-details.component';

const appRoutes: Routes = [
  {
    path: '',
    component: ArtistsContainerComponent,
  },
  {
    path: 'artist-details/:name',
    component: ArtistdetailsComponent,
  },
];

@NgModule({
  declarations: [
    AppComponent,
    ArtistCardComponent,
    ArtistsContainerComponent,
    ArtistdetailsComponent,
  ],
  imports: [
    RouterModule.forRoot(appRoutes),
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
  ],
  providers: [DataService, ArtistService],
  bootstrap: [AppComponent],
})
export class AppModule {}
