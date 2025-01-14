import { Injectable } from "@angular/core";
import { BaseAPIService } from "../../core/base/base-api.service";
import { Observable } from "rxjs";
import { Song } from "../../core/interfaces/song.interface";
import { SongTag } from "../../core/interfaces/song-tag.interface";
import { SongSearchCriteria } from "../../core/interfaces/song-search-criteria.interface";

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

  deleteSong(songId: number): Observable<boolean> {
    return this.delete('/song/' + songId);
  }

  searchSongs(criteria: SongSearchCriteria): Observable<Song[]> {
    return this.post('/song/search', criteria);
  }

  syncSongs(): Observable<Song[]> {
    return this.post('/song/sync', null);
  }
}
