import { Component, OnInit } from "@angular/core";

import { finalize } from "rxjs";
import { Song } from "../../core/interfaces/song.interface";
import { SongTag } from "../../core/interfaces/song-tag.interface";
import { SongTagService } from "../services/song-tag.service";
import { ToastrService } from "ngx-toastr";

@Component({
  selector: 'app-song-tag-list',
  templateUrl: './song-tag-list.component.html'
})
export class SongTagListComponent implements OnInit {
  busy: boolean = false;
  loaded: boolean = false;

  songTags!: SongTag[];

  constructor(private songTagService: SongTagService, private toastr: ToastrService) { }

  // TODO add filtering, tags support, etc
  // TODO add modal for delete

  ngOnInit(): void {
    this.songTagService.getSongTags()
      .pipe(finalize(() => this.loaded = true))
      .subscribe({
        next: songTags => {
          this.songTags = songTags;
        }
      });
  }

  delete(songTag: SongTag) {
    this.busy = true;
    this.songTagService.deleteSongTag(songTag.id as number)
      .pipe(finalize(() => this.busy = false))
      .subscribe({
        next: _ => {
          this.songTags = this.songTags.filter(t => t != songTag);
          this.toastr.success("Tag was deleted");
        }
      })
  }
}
