import { Component, OnInit } from "@angular/core";
import { SongService } from "../services/song.service";
import { finalize, forkJoin } from "rxjs";
import { Song } from "../../core/interfaces/song.interface";
import { ToastrService } from "ngx-toastr";
import { SongTagService } from "../../song-tag/services/song-tag.service";
import { SongTag } from "../../core/interfaces/song-tag.interface";
import { FilterService, SortEvent } from "primeng/api";

@Component({
  selector: 'app-song-list',
  templateUrl: './song-list.component.html'
})
export class SongListComponent implements OnInit {

  loaded: boolean = false;
  busy: boolean = false;

  songs!: Song[];
  tags!: SongTag[];

  constructor(
    private songService: SongService,
    private songTagService: SongTagService,
    private toastr: ToastrService,
    private filterService: FilterService) {
  }

  ngOnInit(): void {
    const observables = [
      this.songService.getSongs(),
      this.songTagService.getSongTags()
    ];

    forkJoin(observables)
      .pipe(finalize(() => this.loaded = true))
      .subscribe({
        next: ([songs, tags]) => {
          this.songs = songs as Song[];
          this.tags = tags as SongTag[];
        }
      });

    this.filterService.register("contains-one-tagId", this.containsOneTagIdFilter);
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

  private containsOneTagIdFilter(value: number[], filter: number[]): boolean {
    if (filter == null || filter.length == 0) {
      return true;
    }

    return value.some(r => filter.includes(r));
  }
}
