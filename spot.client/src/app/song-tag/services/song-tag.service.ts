import { Injectable } from "@angular/core";
import { BaseAPIService } from "../../core/base/base-api.service";
import { Observable } from "rxjs";
import { SongTag } from "../../core/interfaces/song-tag.interface";

@Injectable()
export class SongTagService extends BaseAPIService {
  getSongTag(songTagId: number): Observable<SongTag> {
    return this.get('/songtag/' + songTagId);
  }

  getSongTags(): Observable<SongTag[]> {
    return this.get('/songtag');
  }

  saveSongTag(songTag: SongTag): Observable<SongTag> {
    return this.post('/songtag', songTag);
  }

  deleteSongTag(songTagId: number): Observable<any> {
    return this.delete('/songtag/' + songTagId);
  }
}
