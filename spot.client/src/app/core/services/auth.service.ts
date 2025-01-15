import { Injectable } from "@angular/core";
import { StorageService } from "./storage.service";
import { Observable, Subject, of } from "rxjs";
import { Router } from "@angular/router";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Buffer } from 'buffer';
import { ConfigurationService } from "./configuration.service";

@Injectable()

export class AuthService {
  
  private readonly redirectUri = "https://localhost:4200/callback";
  private readonly scope = "user-read-private user-read-email playlist-read-private user-library-read playlist-modify-public playlist-modify-private";
  private readonly clientId = "27a2d7be9b5349b8aac7ddfa418f3596"; // TODO use config
  private readonly clientSecret = "150d0f76f7df47ff96ffa07c7266c27a"; // TODO use config

  private readonly authorizeUrl = "https://accounts.spotify.com/authorize";
  private readonly tokenUrl = "https://accounts.spotify.com/api/token";

  private readonly spotifyAccessTokenKey = "SPOTIFY_AUTH_CODE";
  private readonly _spotifyAccessToken = new Subject<string>();

  private state!: string;
  
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
    this.config.appSettings
      .subscribe({
        next: appSettings => {
          // TODO use state
          debugger;
          const settings = appSettings.spotifySettings;
          window.location.href = `${this.authorizeUrl}?response_type=code&client_id=${settings.clientId}&scope=${settings.scope}&redirect_uri=${settings.redirectUrl}`;
        }
      });
  }

  postLogin(code: string) {
    this.config.appSettings
      .subscribe({
        next: appSettings => {
          debugger;
          const settings = appSettings.spotifySettings;
          const data = `code=${code}&redirect_uri=${settings.redirectUrl}&grant_type=authorization_code`;

          const headers = new HttpHeaders({
            "content-type": "application/x-www-form-urlencoded",
            "Authorization": 'Basic ' + (Buffer.from(this.clientId + ':' + this.clientSecret).toString('base64'))
          });

          this.http.post(this.tokenUrl, data, { headers })
            .subscribe({
              next: (response: any) => {
                const accessToken = response.access_token;
                this.storageService.store(this.spotifyAccessTokenKey, accessToken);
                this._spotifyAccessToken.next(accessToken);
                this.router.navigate(["/dashboard"]);
              },
              error: error => {
                this.signout();
              }
            });
        }
      });
  }

  signout() {
    this.storageService.delete(this.spotifyAccessTokenKey);
    this.router.navigate(['/home']);
  }
}
