import { HttpClient, HttpErrorResponse, HttpHeaders } from "@angular/common/http";
import { inject } from "@angular/core";
import { Observable, catchError, throwError } from "rxjs";
import { AuthService } from "../services/auth.service";
import { ErrorResult } from "../interfaces/result/error-result.interface";
import { ToastrService } from 'ngx-toastr';
import { ConfigurationService } from "../services/configuration.service";

export abstract class BaseAPIService {

  protected http: HttpClient;
  protected auth: AuthService;
  protected toastr: ToastrService;
  protected config: ConfigurationService;

  constructor() {
    this.http = inject(HttpClient);
    this.auth = inject(AuthService);
    this.toastr = inject(ToastrService);
    this.config = inject(ConfigurationService);
  }

  get<T>(url: string): Observable<T> {
    return this.http.get<T>(this.config.apiUrl + url, { headers: this.getHeaders() })
      .pipe(catchError(error => this.handleErrorResult(error as HttpErrorResponse)));
  }

  post<T, R>(url: string, data: T | null = null): Observable<R> {
    return this.http.post<R>(this.config.apiUrl + url, data, { headers: this.getHeaders() })
      .pipe(catchError(error => this.handleErrorResult(error as HttpErrorResponse)));
  }

  put<T, R>(url: string, data: T | null = null): Observable<R> {
    return this.http.put<R>(this.config.apiUrl + url, data, { headers: this.getHeaders() })
      .pipe(catchError(error => this.handleErrorResult(error as HttpErrorResponse)));
  }

  delete<T>(url: string): Observable<T> {
    return this.http.delete<T>(this.config.apiUrl + url, { headers: this.getHeaders() })
      .pipe(catchError(error => this.handleErrorResult(error as HttpErrorResponse)));
  }

  private getHeaders(): HttpHeaders {
    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/json');
    headers = headers.set("Spotify-Access-Token", this.auth.spotifyAccessToken);
    return headers;
  }

  private handleErrorResult(httpError: HttpErrorResponse): Observable<never> {
    const errorResult = httpError.error as ErrorResult;
    if (!errorResult || !errorResult.errors) {
      this.toastr.error("An unknown error occured");
    } else if (errorResult.isFatal) {
      this.auth.signout();
    } else {
      for (const error of errorResult.errors) {
        this.toastr.error(error);
      }
    }

    return throwError(() => errorResult);
  }
}
