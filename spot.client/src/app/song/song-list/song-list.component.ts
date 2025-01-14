import { Component, OnInit } from "@angular/core";
import { SongService } from "../services/song.service";
import { finalize } from "rxjs";
import { Song } from "../../core/interfaces/song.interface";
import { SongTag } from "../../core/interfaces/song-tag.interface";
import { ToastrService } from "ngx-toastr";

@Component({
  selector: 'app-song-list',
  templateUrl: './song-list.component.html'
})
export class SongListComponent implements OnInit {

  loaded: boolean = false;
  busy: boolean = false;

  songs!: Song[];

  constructor(private songService: SongService, private toastr: ToastrService) { }

  // TODO add filtering, tags support, etc
  // TODO add modal for deleting

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
        },
      });
  }

  delete(song: Song) {
    this.busy = true;
    this.songService.deleteSong(song.id as number)
      .pipe(finalize(() => this.busy = false))
      .subscribe({
        next: _ => {
          this.toastr.success("Song was deleted");
          this.songs = this.songs.filter(s => s != song);
        },
      });
  }
}
