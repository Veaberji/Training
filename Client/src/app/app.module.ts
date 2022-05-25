import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TestComponent } from './test/test.component';
import { DataService } from './services/data.service';
import { ArtistService } from './services/artist.service';
import { ArtistCardComponent } from './artist-card/artist-card.component';
import { ArtistsContainerComponent } from './artists-container/artists-container.component';
import { RouterModule, Routes } from '@angular/router';
import { ArtistsDetailesComponent } from './artists-detailes/artists-detailes.component';

const appRoutes: Routes = [
  {
    path: '',
    component: ArtistsContainerComponent,
  },
  {
    path: 'artist-details/:id',
    component: ArtistsDetailesComponent,
  },
];

@NgModule({
  declarations: [
    AppComponent,
    TestComponent,
    ArtistCardComponent,
    ArtistsContainerComponent,
    ArtistsDetailesComponent,
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
