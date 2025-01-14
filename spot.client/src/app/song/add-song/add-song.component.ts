import { Component, OnInit } from "@angular/core";
import { SongService } from "../services/song.service";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { SongSearchCriteria } from "../../core/interfaces/song-search-criteria.interface";
import { Song } from "../../core/interfaces/song.interface";
import { ToastrService } from "ngx-toastr";
import { finalize } from "rxjs";

@Component({
  selector: 'app-add-song',
  templateUrl: './add-song.component.html'
})
export class AddSongComponent implements OnInit {
  loaded: boolean = false;
  busy: boolean = false;
  form!: FormGroup;
  songs: Song[] | null = null;

  // TODO add form validation for all forms

  constructor(
    private songService: SongService,
    private toastr: ToastrService,
    private fb: FormBuilder,
  ) {
  }

  ngOnInit(): void {
    this.form = this.fb.group({
      name: [null, Validators.required],
      artist: [null],
      album: [null]
    });

    this.loaded = true;
  }

  search() {
    if (!this.form.valid) {
      return;
    }

    this.busy = true;
    const searchCriteria = this.form.getRawValue() as SongSearchCriteria;
    this.songService.searchSongs(searchCriteria)
      .pipe(finalize(() => this.busy = false))
      .subscribe({
        next: songs => {
          this.songs = songs;
          if (this.songs == null || this.songs.length == 0) {
            this.songs = null;
            this.toastr.info("No songs found");
          }
        }
      });
  }

  add(song: Song) {
    this.busy = true;
    this.songService.saveSong(song)
      .pipe(finalize(() => this.busy = false))
      .subscribe({
        next: _ => {
          if (this.songs) {
            this.toastr.success("Song added");
            this.songs = this.songs.filter(s => s != song);
          }
        }
      })
  }
}
