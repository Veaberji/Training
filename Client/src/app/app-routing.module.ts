import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ArtistRoutingModule } from './artists/artist-routing.module';
import { NotFoundComponent } from './shared/not-found/not-found.component';

const routes: Routes = [
  { path: '', redirectTo: '/artists', pathMatch: 'full' },
  {
    path: 'artists',
    loadChildren: () => import('./artists/artist-routing.module').then((m) => m.ArtistRoutingModule),
  },
  {
    path: 'albums',
    loadChildren: () => import('./albums/album-routing.module').then((m) => m.AlbumRoutingModule),
  },
  {
    path: '**',
    component: NotFoundComponent,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes), ArtistRoutingModule],
  exports: [RouterModule],
})
export class AppRoutingModule {}
