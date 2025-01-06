import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, map, Observable, of, tap, throwError } from 'rxjs';
import { GenericHttpClientService } from './generic-http-client.service';
import { AuthResponse, LoginCredentials } from '../api/auth/login/login-request';
import { User } from '../api/auth/user';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  _baseEndPoint: string = 'Tokens';
  constructor(private _httpService: GenericHttpClientService, private router: Router) {
    this.initializeTokensFromStorage();
  }
  //login method
  login(request: LoginCredentials): Observable<any> {
    return this._httpService.post<any>(`${this._baseEndPoint}/get-token`, undefined, request)
  }

  private tokenSubject = new BehaviorSubject<string | null>(null);
  private refreshTokenSubject = new BehaviorSubject<string | null>(null);
  private refreshExpirySubject = new BehaviorSubject<string | null>(null);
  // current user
  private user = new BehaviorSubject<User | null>(null);

  public initializeTokensFromStorage(): void {
    const storedToken = localStorage.getItem('access_token');
    const storedRefreshToken = localStorage.getItem('refresh_token');
    const storedRefreshExpiry = localStorage.getItem('refresh_expiry');

    if (storedToken) {
      this.tokenSubject.next(storedToken);
      // initialize user
      this.user.next(JSON.parse(atob(storedToken.split('.')[1])) as User);
    }
    if (storedRefreshExpiry) {
      this.refreshExpirySubject.next(storedRefreshExpiry);
    }
    if (storedRefreshToken) {
      this.refreshTokenSubject.next(storedRefreshToken);
    }
  }

  // isAuthenticated
  get isAuthenticated(): boolean {
    return this.isRefreshTokenValid();
  }
  // Getter for current token
  get currentToken(): string | null {
    return this.tokenSubject.value;
  }

  // Getter for User
  get User(): User | null {
    return this.user.value;
  }

  // Comprehensive token expiry checking
  isRefreshTokenValid(): boolean {
    const refreshExpiry = this.refreshExpirySubject.value;

    // No refresh token expiry found
    if (!refreshExpiry) {
      return false;
    }

    // Parse the expiry date
    const expiryDate = new Date(refreshExpiry);
    const currentDate = new Date();

    // Check if refresh token is expired
    return expiryDate > currentDate;
  }

  // Advanced token validation method
  validateTokens(): 'valid' | 'access_expired' | 'refresh_expired' | 'both_expired' {
    const isAccessTokenExpired = this.isTokenExpired();
    const isRefreshTokenValid = this.isRefreshTokenValid();

    if (!isAccessTokenExpired) {
      return 'valid'; // Access token is still valid
    }

    if (isRefreshTokenValid) {
      return 'access_expired'; // Can refresh token
    }

    if (!isRefreshTokenValid) {
      return 'refresh_expired'; // Cannot refresh, full re-authentication needed
    }

    return 'both_expired';
  }
  // Check if token is expired
  isTokenExpired(): boolean {
    const token = this.tokenSubject.value;
  
    if (!token) return true;

    try {
      const tokenPayload = JSON.parse(atob(token.split('.')[1]));
      const expirationTime = tokenPayload.exp * 1000; // Convert to milliseconds

      return Date.now() >= expirationTime;
    } catch (error) {
      return true;
    }
  }
  // Refresh token method with expiry validation
  refreshToken(): Observable<AuthResponse> {
    const refreshToken = this.refreshTokenSubject.value;

    // Check refresh token validity before attempting refresh
    if (!this.isRefreshTokenValid()) {
      return throwError(() => new Error('Session has expired.'));
    }

    if (!refreshToken) {
      return throwError(() => new Error('Your are no more login.'));
    }
    const token = this.tokenSubject.value;
    return this._httpService.post<AuthResponse>(`${this._baseEndPoint}/refresh`, undefined, { refreshToken, token }).pipe(
      tap(response => {
        // Validate new tokens after refresh
        this.setSession(response);
      }),
      catchError(error => {
        // Comprehensive error handling
        console.error('Token refresh failed', error);
        this.logout();
        this.router.navigate(['login']);
        return throwError(() => new Error('Token refresh failed. Please log in again.'));
      })
    );
  }

  // Automatic token management method
  ensureValidToken(): Observable<string> {
  
    const tokenStatus = this.validateTokens();
    switch (tokenStatus) {
      case 'valid':
        return of(this.currentToken as string);

      case 'access_expired':
        // Try to refresh token
        const accessToken = this.refreshToken().pipe(
          map(response => response.token),
          catchError(() => {
            // If refresh fails, force logout
            this.router.navigate(["/auth"])
            this.logout();
            throw new Error('Token refresh failed');
          })
        );

        return accessToken;


      case 'refresh_expired':
      case 'both_expired':
        // Force user to log in again
        this.logout();
        throw new Error('Authentication expired. Please log in again.');
    }
  }

  // Enhanced logout to handle token invalidation
  logout(): void {
    // Clear all token-related storage and subjects
    localStorage.removeItem('access_token');
    localStorage.removeItem('refresh_token');
    localStorage.removeItem('refresh_expiry');

    this.tokenSubject.next(null);
    this.refreshTokenSubject.next(null);
    this.refreshExpirySubject.next(null);
    this.user.next(null);
  }
  // Set session after successful login
  public setSession(authResponse: AuthResponse): void {
    // Store tokens in local storage
    localStorage.setItem('access_token', authResponse.token);
    localStorage.setItem('refresh_token', authResponse.refreshToken);
    localStorage.setItem('refresh_expiry', authResponse.refreshTokenExpiryTime);

    // Update token subjects
    this.tokenSubject.next(authResponse.token);
    this.refreshTokenSubject.next(authResponse.refreshToken);
    this.refreshExpirySubject.next(authResponse.refreshTokenExpiryTime);
  }
}
