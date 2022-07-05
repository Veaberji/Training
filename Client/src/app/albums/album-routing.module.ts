import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AlbumDetailsComponent } from './album-details/album-details.component';
import { AlbumDetailsResolver } from './services/album-details-resolver.service';

const routes: Routes = [
  {
    path: ':artistName/:albumTitle/details',
    component: AlbumDetailsComponent,
    resolve: { resolvedData: AlbumDetailsResolver },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
})
export class AlbumRoutingModule {}
