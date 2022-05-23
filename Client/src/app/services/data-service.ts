import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DataService {
  constructor(private http: HttpClient) {}

  getAll() {
    return this.http
      .get('https://localhost:7093/weather')
      .pipe(map((response) => response as any[]));
  }
}
