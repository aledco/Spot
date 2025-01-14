import { HttpClient, HttpErrorResponse, HttpHeaders } from "@angular/common/http";
import { inject } from "@angular/core";
import { Observable, catchError, map, mergeMap, throwError } from "rxjs";
import { AuthService } from "../services/auth.service";
import { ErrorResult } from "../interfaces/result/error-result.interface";
import { ToastrService } from 'ngx-toastr';

export abstract class BaseAPIService {

  protected http: HttpClient;
  protected auth: AuthService;
  protected toastr: ToastrService;
  private readonly baseUrl = "https://localhost:7108/api"; // TODO read from config
  //private readonly baseUrl = "/api";

  constructor() {
    this.http = inject(HttpClient);
    this.auth = inject(AuthService);
    this.toastr = inject(ToastrService);
  }

  get<T>(url: string): Observable<T> {
    const accessToken = this.auth.spotifyAccessToken;
    return this.http.get<T>(this.baseUrl + url + `?spotifyAccessToken=${accessToken}`, { headers: this.getHeaders() })
      .pipe(catchError(error => this.handleErrorResult(error as HttpErrorResponse)));
  }

  post<T, R>(url: string, data: T): Observable<R> {
    const accessToken = this.auth.spotifyAccessToken;
    return this.http.post<R>(this.baseUrl + url + `?spotifyAccessToken=${accessToken}`, data, { headers: this.getHeaders() })
      .pipe(catchError(error => this.handleErrorResult(error as HttpErrorResponse)));
  }

  private getHeaders(): HttpHeaders {
    const headers = new HttpHeaders();
    headers.set('Content-Type', 'application/json');
    return headers;
  }

  private handleErrorResult(httpError: HttpErrorResponse) {
    const errorResult = httpError.error as ErrorResult;
    if (!errorResult || errorResult.isFatal) {
      this.auth.home();
    } else {
      for (const error of errorResult.errors) {
        this.toastr.error(error);
      }
    }

    return throwError(errorResult);
  }
}
