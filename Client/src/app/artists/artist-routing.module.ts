import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ArtistDetailsComponent } from './artist-details/artist-details.component';
import { ArtistsContainerComponent } from './artists-container/artists-container.component';

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
