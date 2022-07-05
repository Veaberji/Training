import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
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
    path: 'not-found',
    component: NotFoundComponent,
  },
  { path: '**', redirectTo: '/not-found', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
