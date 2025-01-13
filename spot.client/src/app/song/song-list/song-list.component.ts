import { Component, OnInit } from "@angular/core";
import { SongService } from "../services/song.service";
import { finalize } from "rxjs";
import { Song } from "../../core/interfaces/song.interface";
import { SongTag } from "../../core/interfaces/song-tag.interface";

@Component({
  selector: 'app-song-list',
  templateUrl: './song-list.component.html'
})
export class SongListComponent implements OnInit {

  loaded: boolean = false;
  busy: boolean = false;

  songs!: Song[];

  constructor(private songService: SongService) { }

  // TODO add filtering, tags support, etc

  ngOnInit(): void {
    this.songService.getSongs()
      .pipe(finalize(() => this.loaded = true))
      .subscribe({
        next: songs => {
          this.songs = songs as Song[];
        }
      });
  }

  sync() {
    this.busy = true;
    this.songService.syncSongs()
      .pipe(finalize(() => this.busy = false))
      .subscribe({
        next: songs => {
          this.songs = songs;
        }
      });
  }
}
