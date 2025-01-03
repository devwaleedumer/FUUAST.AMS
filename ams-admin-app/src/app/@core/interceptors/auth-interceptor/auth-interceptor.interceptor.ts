import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, from, throwError, firstValueFrom, catchError, switchMap, finalize } from 'rxjs';
import { AuthService } from '../../utilities/auth-service.service';
import { LoadingService } from '../../services/loading/loading.service';
const gracePaths = [
  "get-token",
  "refresh",
  "confirm-mail-set-password"
];
@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService, private loader: LoadingService) { }
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    this.loader.show();
    const url = !request.url.includes('?') ? request.url.split('/').pop() : request.url.split('?')[0].split('/').pop();
    if (gracePaths.includes(url as string)) {
      return next.handle(request).pipe(
        finalize(() => {
          this.loader.hide();
        }));;
    }
    else
      // Convert ensureValidToken to work with HttpInterceptor
      return from(this.addAuthToken(request)).pipe(
        switchMap(authorizedRequest => next.handle(authorizedRequest)),
        catchError((error: HttpErrorResponse) => {
          // Handle specific authentication errors
          if (error.message === "Invalid Token.") {
            // Force token validation and retry
            return this.handleUnauthorizedError(request, next);
          }
          return throwError(() => error);
        })
      );
  }

  private async addAuthToken(request: HttpRequest<any>): Promise<HttpRequest<any>> {
    try {
      const token = await firstValueFrom(this.authService.ensureValidToken());
      return request.clone({
        setHeaders: { Authorization: `Bearer ${token}` }
      });
    } catch (error) {
      // If token validation fails, rethrow error
      throw error;
    }
  }
  private handleUnauthorizedError(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    // Attempt to refresh token and retry request
    return from(this.authService.ensureValidToken()).pipe(
      switchMap(newToken => {
        // Clone request with new token
        const authorizedRequest = request.clone({
          setHeaders: { Authorization: `Bearer ${newToken}` }
        });
        return next.handle(authorizedRequest);
      }),
      catchError(err => {
        // If all token refresh attempts fail
        this.authService.logout();
        return throwError(() => err);
      })
    );
  }
}
