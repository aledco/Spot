import { Injectable } from "@angular/core";
import { StorageService } from "./storage.service";
import { Subject } from "rxjs";
import { Router } from "@angular/router";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Buffer } from 'buffer';
import { v4 as uuid } from 'uuid';
import { ConfigurationService } from "./configuration.service";
import { environment } from "../../../environments/environment";

@Injectable()
export class AuthService {
  private readonly authorizeUrl = "https://accounts.spotify.com/authorize";

  private readonly spotifyAccessTokenKey = "SPOTIFY_AUTH_CODE";
  private readonly _spotifyAccessToken = new Subject<string>();

  private readonly stateKey = "SPOTIFY_STATE";

  constructor(
    private storageService: StorageService,
    private config: ConfigurationService,
    private http: HttpClient,
    private router: Router) { }

  get spotifyAccessToken(): string {
    const accessToken = this.storageService.get<string>(this.spotifyAccessTokenKey);
    if (!accessToken) {
      this.login();
      return "";
    }

    return accessToken;
  }

  get isLoggedIn(): boolean {
    return this.storageService.has(this.spotifyAccessTokenKey);
  }

  login(): void {
    const state = uuid();
    this.storageService.store(this.stateKey, state);

    this.config.appSettings
      .subscribe({
        next: appSettings => {
          const settings = appSettings.spotifySettings;
          window.location.href = `${this.authorizeUrl}?response_type=code&client_id=${settings.clientId}&scope=${settings.scope}&redirect_uri=${settings.redirectUrl}&state=${state}`;
        }
      });
  }

  postLogin(code: string, state: string) {
    const storedState = this.storageService.get(this.stateKey);
    if (state != storedState) {
      this.signout();
    }

    this.storageService.delete(this.stateKey);

    this.http.get<string>(this.config.apiUrl + '/auth/spotify/' + code)
      .subscribe({
        next: accessToken => {
          this.storageService.store(this.spotifyAccessTokenKey, accessToken);
          this._spotifyAccessToken.next(accessToken);
          this.router.navigate(["/dashboard"]);
        }
      });
  }

  signout() {
    this.storageService.delete(this.spotifyAccessTokenKey);
    this.router.navigate(['/home']);
  }
}
