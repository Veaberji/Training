import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AlbumDetailsComponent } from './album-details/album-details.component';

const routes: Routes = [
  {
    path: ':artistName/:albumTitle/details',
    component: AlbumDetailsComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
})
export class AlbumRoutingModule {}
