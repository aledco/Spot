import { Component, OnInit } from "@angular/core";

import { finalize } from "rxjs";
import { Song } from "../../core/interfaces/song.interface";
import { SongTag } from "../../core/interfaces/song-tag.interface";
import { SongTagService } from "../services/song-tag.service";

@Component({
  selector: 'app-song-tag-list',
  templateUrl: './song-tag-list.component.html'
})
export class SongTagListComponent implements OnInit {

  loaded: boolean = false;

  songTags!: SongTag[];

  constructor(private songTagService: SongTagService) { }

  // TODO add filtering, tags support, etc

  ngOnInit(): void {
    this.songTagService.getSongTags()
      .pipe(finalize(() => this.loaded = true))
      .subscribe({
        next: songTags => {
          this.songTags = songTags;
        }
      });
  }
}
