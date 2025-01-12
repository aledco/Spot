import { Component, OnInit } from "@angular/core";
import { SongService } from "../services/song.service";
import { finalize, forkJoin } from "rxjs";
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
  tags!: SongTag[];

  constructor(private songService: SongService) { }

  // TODO add filtering, tags support, etc

  ngOnInit(): void {
    const observables = [
      this.songService.getSongs(),
      this.songService.getSongTags()
    ];

    forkJoin(observables)
      .pipe(finalize(() => this.loaded = true))
      .subscribe({
        next: ([songs, tags]) => {
          this.songs = songs as Song[];
          this.tags = tags as SongTag[];
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
