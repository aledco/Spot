import { Injectable } from "@angular/core";
import { BaseAPIService } from "../../core/base/base-api.service";
import { Observable } from "rxjs";
import { SpotifyPlaylistsResult } from "../../core/interfaces/spotify/spotify-playlist-result.interface";

@Injectable()
export class PlaylistService extends BaseAPIService {
  getPlaylists(): Observable<SpotifyPlaylistsResult> {
    return this.get<SpotifyPlaylistsResult>('/Playlist');
  }
}
