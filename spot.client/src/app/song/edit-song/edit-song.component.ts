import { Component, OnInit } from "@angular/core";
import { SongService } from "../services/song.service";
import { Song } from "../../core/interfaces/song.interface";
import { SongTag } from "../../core/interfaces/song-tag.interface";
import { ActivatedRoute } from "@angular/router";
import { finalize, forkJoin } from "rxjs";
import { SongTagService } from "../../song-tag/services/song-tag.service";

@Component({
  selector: 'app-edit-song',
  templateUrl: './edit-song.component.html'
})
export class EditSongComponent implements OnInit {
  loaded: boolean = false;
  busy: boolean = false;
  song!: Song;
  tags!: SongTag[];

  constructor(
    private songService: SongService,
    private songTagService: SongTagService,
    private route: ActivatedRoute
  ) {
  }

  ngOnInit() {
    const songId = this.route.snapshot.paramMap.get('songId');
    if (!songId) {
      return;
    }

    const observables = [
      this.songService.getSong(+songId),
      this.songTagService.getSongTags()
    ];

    forkJoin(observables)
      .pipe(finalize(() => this.loaded = true))
      .subscribe({
        next: ([song, tags]) => {
          this.song = song as Song;
          this.tags = tags as SongTag[];
        }
      });
  }

  save() {
    this.busy = true;
    this.songService.saveSong(this.song)
      .pipe(finalize(() => this.busy = false))
      .subscribe({
        next: song => {
          this.song = song;
        }
      });
  }
}
