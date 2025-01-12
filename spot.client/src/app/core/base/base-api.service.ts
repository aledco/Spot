import { HttpClient, HttpHeaders } from "@angular/common/http";
import { inject } from "@angular/core";
import { Observable, mergeMap } from "rxjs";
import { AuthService } from "../services/auth.service";

export abstract class BaseAPIService {

  protected http: HttpClient;
  protected auth: AuthService;
  private readonly baseUrl = "https://localhost:7108/api"; // TODO read from config
  //private readonly baseUrl = "/api";

  constructor() {
    this.http = inject(HttpClient);
    this.auth = inject(AuthService);
  }

  get<T>(url: string): Observable<T> {
    return this.auth.spotifyAccessToken.pipe(mergeMap(accessToken => {
      return this.http.get<T>(this.baseUrl + url + `?spotifyAccessToken=${accessToken}`);
    }));
  }

  post<T, R>(url: string, data: T): Observable<R> {
    return this.auth.spotifyAccessToken.pipe(mergeMap(accessToken => {
      return this.http.post<R>(this.baseUrl + url + `?spotifyAccessToken=${accessToken}`, data);
    }));
  }

  private getHeaders(): HttpHeaders {
    return new HttpHeaders({
      "Content-Type": "application/json",
      "Access-Control-Allow-Origin": "*"
    });
  }
}
