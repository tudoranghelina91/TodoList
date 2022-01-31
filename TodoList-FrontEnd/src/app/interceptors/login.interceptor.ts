import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable()
export class LoginInterceptor implements HttpInterceptor {

  constructor(private router : Router) {}

  handleError(error : HttpErrorResponse) {
    if (error.status != 401) {
      return;
    }
    
    localStorage.clear();
    this.router.navigateByUrl('login');
    return throwError(error);
  }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    request = request.clone({
      setHeaders: {
        'Authorization': `${localStorage.getItem('accessToken')}`,
      },
    });

    return next.handle(request)
    .pipe(catchError(err => this.handleError(err)));
  }
}
