import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TopAlbumsContainerComponent } from '../albums/albums-container/albums-container.component';
import { TopTrackListComponent } from '../tracks/track-list/track-list.component';
import { ArtistDetailsComponent } from './artist-details/artist-details.component';
import { ArtistsContainerComponent } from './artists-container/artists-container.component';
import { SimilarArtistsComponent } from './similar-artists/similar-artists.component';
import { SupplementRoute } from './supplementRoutes';

const routes: Routes = [
  {
    path: '',
    component: ArtistsContainerComponent,
  },
  {
    path: 'page/:page',
    component: ArtistsContainerComponent,
  },
  {
    path: 'page/:page/pageSize/:pageSize',
    component: ArtistsContainerComponent,
  },
  {
    path: 'details/:name',
    component: ArtistDetailsComponent,
    children: [
      {
        path: '',
        redirectTo: SupplementRoute.TopTracks,
        pathMatch: 'full',
      },
      {
        path: SupplementRoute.TopTracks,
        component: TopTrackListComponent,
      },
      {
        path: SupplementRoute.TopAlbums,
        component: TopAlbumsContainerComponent,
      },
      {
        path: SupplementRoute.SimilarArtists,
        component: SimilarArtistsComponent,
      },
    ],
  },
  {
    path: 'details/:name/:supplement',
    component: ArtistDetailsComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
})
export class ArtistRoutingModule {}
