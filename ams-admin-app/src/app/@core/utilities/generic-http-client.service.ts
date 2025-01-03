import { Injectable } from '@angular/core';
import {HttpClient, HttpErrorResponse, HttpHeaders, HttpParams} from '@angular/common/http';
import {catchError, map, Observable, throwError} from 'rxjs';


// Generic interface for API responses
interface ApiResponse<T> {
  data?: T;
  message?: string;
  status?: number;
  errors?: any;
}
@Injectable({
  providedIn: 'root'
})
export class GenericHttpClientService {

  // base api url
  private apiUrl = 'https://localhost:7081/api/';
  constructor(private http: HttpClient) {}
  // Error handler
  // Error handling
  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'An error occurred';
  debugger;
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = error.error.message;
    } else {
      // Server-side error
      errorMessage = error.error?.detail || `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    console.error(errorMessage);
    return throwError(() => new Error(errorMessage));
  }

  // Configure default headers
  private getHeaders(): HttpHeaders {
    return new HttpHeaders({
      'Content-Type': 'application/json',
      // Add any default headers here
    });
  }
// Set Query Parameters
  private setQueryParams (params:{[key: string]: any}): HttpParams {
    let  httpParams: HttpParams = new HttpParams();
    Object.keys(params).forEach(key => {
      if (params[key] !== null && params[key] !== undefined) {
        httpParams = httpParams.set(key, params[key]);
      }
    });
      return httpParams;
  }
  // Generic GET request
  public get<T>(
    endpoint: string,
    params?: { [key: string]: any },
    customHeaders?: HttpHeaders,
    reportProgress?: boolean,
    responseType?: 'json',
    withCredentials?: boolean
  ): Observable<T> {
    const headers = customHeaders || this.getHeaders();
    let httpParams = new HttpParams();

    if (params) {
     httpParams = this.setQueryParams(params);
    }
    return this.http.get<T>(`${this.apiUrl}${endpoint}`, {
      headers,
      params: httpParams,
      reportProgress,
      responseType,
      withCredentials
    }).pipe(
      map(response => response as T),
      catchError(this.handleError)
    );
  }

  // Generic DELETE request
  public delete<T>(
    endpoint: string,
    customHeaders?: HttpHeaders,
    reportProgress?: boolean,
    responseType?: 'json',
    withCredentials?: boolean
  ): Observable<T> {
    const headers = customHeaders || this.getHeaders();

    return this.http.delete<T>(`${this.apiUrl}${endpoint}`, {
      headers,
      reportProgress,
      responseType,
      withCredentials
    }).pipe(
      map(response => response as T),
      catchError(this.handleError)
    );
  }
  public  post<T>(
    endpoint: string,
    customHeaders?: HttpHeaders ,
    body?: any,
    reportProgress?: boolean,
    responseType?: 'json',
    withCredentials?: boolean
  ) : Observable<T>
  {
    const headers = customHeaders || this.getHeaders();
    return  this.http.post<T>(`${this.apiUrl}${endpoint}`, body,{
      headers,
      reportProgress,
      responseType:responseType as any,
      withCredentials},)
      .pipe(map(response => response as T),catchError(this.handleError))
  }

  // Generic PUT request
  public put<T>(
    endpoint: string,
    body: any,
    customHeaders?: HttpHeaders,
    reportProgress?: boolean,
    responseType?: 'json',
    withCredentials?: boolean
  ): Observable<T> {
    const headers = customHeaders || this.getHeaders();

    return this.http.put<T>(`${this.apiUrl}${endpoint}`, body, {
      headers,
      reportProgress,
      responseType,
      withCredentials
    }).pipe(
      map(response => response as T),
      catchError(this.handleError)
    );
  }
}
