import { Injectable } from "@angular/core";
import { BaseAPIService } from "../../core/base/base-api.service";
import { Observable } from "rxjs";
import { Song } from "../../core/interfaces/song.interface";

@Injectable()
export class SongService extends BaseAPIService {
  getSongs(): Observable<Song[]> {
    return this.get<any[]>('/song');
  }

  syncSongs(): Observable<Song[]> {
    return this.post('/song/sync', null);
  }
}
