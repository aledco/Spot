import { Injectable } from "@angular/core";
import { BaseAPIService } from "../../core/base/base-api.service";
import { Observable } from "rxjs";
import { Song } from "../../core/interfaces/song.interface";
import { SongTag } from "../../core/interfaces/song-tag.interface";

@Injectable()
export class SongTagService extends BaseAPIService {
  getSongTag(songTagId: number): Observable<SongTag> {
    return this.get('/songtag/' + songTagId);
  }

  getSongTags(): Observable<SongTag[]> {
    return this.get('/songtag');
  }

  saveSongTag(songTag: SongTag): Observable<Song> {
    return this.post('/songtag', songTag);
  }
}
