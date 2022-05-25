import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { map, throwError } from 'rxjs';
import { catchError } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DataService<T> {
  constructor(
    @Inject(String) private baseUrl: string,
    private http: HttpClient
  ) {}

  getAll(query?: string) {
    const url = query ? `${this.baseUrl}${query}` : this.baseUrl;

    return this.http.get(url).pipe(
      map((response) => response as T[]),
      catchError((error: any) => {
        console.log(error);
        return throwError(() => new Error(error));
      })
    );
  }

  handleError(error: any) {
    console.log(error);
    return throwError(() => new Error(error));
  }
}
