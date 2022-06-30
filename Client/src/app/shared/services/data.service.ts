import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { map, Observable, throwError, catchError } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DataService<T> {
  constructor(@Inject(String) private baseUrl: string, private http: HttpClient) {}

  getAll(query?: string): Observable<T[]> {
    const url = query ? `${this.baseUrl}${query}` : this.baseUrl;

    return this.http.get(url).pipe(
      map((response) => response as T[]),
      catchError((error: any) => {
        console.log(error);
        return throwError(() => new Error(error));
      })
    );
  }

  get(selector: string): Observable<T> {
    const url = `${this.baseUrl}${selector}`;

    return this.http.get(url).pipe(
      map((response) => response as T),
      catchError((error: any) => {
        console.log(error);
        return throwError(() => new Error(error));
      })
    );
  }

  handleError(error: any): Observable<any> {
    console.log(error);
    return throwError(() => new Error(error));
  }
}
