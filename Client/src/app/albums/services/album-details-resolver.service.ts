import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { catchError, EMPTY, Observable } from 'rxjs';
import { AppErrorHandler } from 'src/app/shared/common/errors/app-error-handler';
import { AlbumDetails } from '../models/album-details';
import { AlbumDetailsService } from './album-details.service';

@Injectable({
  providedIn: 'root',
})
export class AlbumDetailsResolver implements Resolve<AlbumDetails> {
  constructor(private service: AlbumDetailsService, private errorHandler: AppErrorHandler) {}

  resolve(route: ActivatedRouteSnapshot, _: RouterStateSnapshot): Observable<AlbumDetails> {
    let params = route.paramMap;
    const albumTitle = String(params.get('albumTitle'));
    const artistName = String(params.get('artistName'));

    return this.service.getAlbumDetails(artistName, albumTitle).pipe(
      catchError((error) => {
        this.errorHandler.handleError(error as Error);
        return EMPTY;
      })
    );
  }
}
