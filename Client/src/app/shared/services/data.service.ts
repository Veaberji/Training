import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { map, Observable, throwError, catchError, switchMap } from 'rxjs';
import { AppError } from '../common/errors/app-error';
import { NotFoundError } from '../common/errors/not-found-error';
import { ServerError } from '../common/errors/server-error';

@Injectable({
  providedIn: 'root',
})
export class DataService<T> {
  constructor(@Inject(String) private baseUrl: string, private http: HttpClient) {}

  getAll(query?: string): Observable<T[]> {
    const url = query ? `${this.baseUrl}${query}` : this.baseUrl;

    return this.http.get(url).pipe(
      map((response) => response as T[]),
      catchError(this.handleError)
    );
  }

  getAllByObservable(query$: Observable<string>): Observable<T[]> {
    return query$.pipe(
      switchMap((query) => {
        const url = query ? `${this.baseUrl}${query}` : this.baseUrl;
        return this.http.get(url).pipe(
          map((response) => response as T[]),
          catchError(this.handleError)
        );
      })
    );
  }

  get(selector: string): Observable<T> {
    const url = `${this.baseUrl}${selector}`;

    return this.http.get(url).pipe(
      map((response) => response as T),
      catchError(this.handleError)
    );
  }

  getByObservable(selector$: Observable<string>): Observable<T> {
    return selector$.pipe(
      switchMap((selector) => {
        const url = `${this.baseUrl}${selector}`;
        return this.http.get(url).pipe(
          map((response) => response as T),
          catchError(this.handleError)
        );
      })
    );
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    if (error.status === 404) {
      return throwError(() => new NotFoundError(`Data Not Found: ${error.error}`));
    } else if (error.status >= 500) {
      return throwError(() => new ServerError(`Server returned code ${error.status}: ${error.error}`));
    } else {
      return throwError(() => new AppError('An unexpected error occurred'));
    }
  }
}
