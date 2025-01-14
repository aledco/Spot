import { Component, OnInit } from "@angular/core";
import { SongTag } from "../../core/interfaces/song-tag.interface";
import { ActivatedRoute } from "@angular/router";
import { finalize } from "rxjs";
import { SongTagService } from "../services/song-tag.service";

@Component({
  selector: 'app-add-or-edit-song-tag',
  templateUrl: './add-or-edit-song-tag.component.html'
})
export class AddOrEditSongTagComponent implements OnInit {
  loaded: boolean = false;
  busy: boolean = false;
  songTag!: SongTag;

  constructor(
    private songTagService: SongTagService,
    private route: ActivatedRoute
  ) {
  }

  // TODO add description and isPublic to form

  ngOnInit() {
    const songTagId = this.route.snapshot.paramMap.get('songTagId');
    if (songTagId) {
      this.songTagService.getSongTag(+songTagId)
        .pipe(finalize(() => this.loaded = true))
        .subscribe({
          next: tag => {
            this.songTag = tag;
          }
        });
    } else {
      this.songTag = {} as SongTag;
      this.loaded = true;
    }
  }

  save() {
    this.busy = true;
    this.songTagService.saveSongTag(this.songTag)
      .pipe(finalize(() => this.busy = false))
      .subscribe({
        next: tag => {
          this.songTag = tag;
        }
      });
  }
}
