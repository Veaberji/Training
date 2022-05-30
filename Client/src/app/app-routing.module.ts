import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AlbumDetailsComponent } from './album-details/album-details.component';
import { ArtistDetailsComponent } from './artist-details/artist-details.component';
import { ArtistsContainerComponent } from './artists-container/artists-container.component';
import { NotFoundComponent } from './common/not-found/not-found.component';
import { supplementRoutes } from './supplementRoutes';
import { TrackListComponent } from './track-list/track-list.component';

const routes: Routes = [
  {
    path: '',
    component: ArtistsContainerComponent,
  },
  {
    path: 'artist-details/:name',
    component: ArtistDetailsComponent,
  },
  {
    path: 'artist-details/:name/:supplement',
    component: ArtistDetailsComponent,
  },
  {
    path: ':artistName/album-details/:albumTitle',
    component: AlbumDetailsComponent,
  },
  {
    path: '**',
    component: NotFoundComponent,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
