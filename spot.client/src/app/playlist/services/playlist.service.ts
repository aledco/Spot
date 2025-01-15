import { Injectable } from "@angular/core";
import { BaseAPIService } from "../../core/base/base-api.service";
import { Observable } from "rxjs";
import { SimplifiedSpotifyPlaylist } from "../../core/interfaces/spotify/spotify-simplified-playlist.interface";

@Injectable()
export class PlaylistService extends BaseAPIService {
  getPlaylists(): Observable<SimplifiedSpotifyPlaylist[]> {
    return this.get<SimplifiedSpotifyPlaylist[]>('/playlist');
  }

  shufflePlaylist(playlistId: string): Observable<any> {
    return this.put(`/playlist/${playlistId}/shuffle`);
  }
}
