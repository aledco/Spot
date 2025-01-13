import { Injectable } from "@angular/core";
import { BaseAPIService } from "../../core/base/base-api.service";
import { Observable } from "rxjs";
import { Song } from "../../core/interfaces/song.interface";
import { SongTag } from "../../core/interfaces/song-tag.interface";

@Injectable()
export class SongService extends BaseAPIService {
  getSong(songId: number): Observable<Song> {
    return this.get('/song/' + songId);
  }

  getSongs(): Observable<Song[]> {
    return this.get('/song');
  }

  saveSong(song: Song): Observable<Song> {
    return this.post('/song', song);
  }

  syncSongs(): Observable<Song[]> {
    return this.post('/song/sync', null);
  }
}
