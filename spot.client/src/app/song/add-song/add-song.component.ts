import { Component, OnInit } from "@angular/core";
import { SongService } from "../services/song.service";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { SongSearchCriteria } from "../../core/interfaces/song-search-criteria.interface";

@Component({
  selector: 'app-add-song',
  templateUrl: './add-song.component.html'
})
export class AddSongComponent implements OnInit {
  loaded: boolean = false;
  form!: FormGroup;

  constructor(
    private songService: SongService,
    private fb: FormBuilder,
  ) {
  }

  ngOnInit(): void {
    this.form = this.fb.group({
      name: [null, Validators.required],
      artist: [null],
      album: [null]
    });
  }

  search() {
    const searchCriteria = this.form.getRawValue() as SongSearchCriteria;

    // TODO perform search
  }
}
