import { ErrorHandler, Injectable, NgZone } from '@angular/core';
import { Router } from '@angular/router';
import { NotFoundError } from './not-found-error';
import { ServerError } from './server-error';

@Injectable({
  providedIn: 'root',
})
export class AppErrorHandler extends ErrorHandler {
  constructor(private router: Router, private zone: NgZone) {
    super();
  }
  override handleError(error: Error): void {
    console.log(error.message);

    if (error instanceof NotFoundError) {
      this.zone.run(() => this.router.navigate(['/not-found']));
    } else if (error instanceof ServerError) {
      alert(error.message);
    }
  }
}
