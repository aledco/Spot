import { Component, OnInit } from "@angular/core";
import { SongService } from "../services/song.service";

@Component({
  selector: 'app-song-list',
  templateUrl: './song-list.component.html'
})
export class SongListComponent implements OnInit {

  songs!: any[];

  constructor(private songService: SongService) { }

  ngOnInit(): void {
    this.songService.getSongs()
      .subscribe({
        next: songs => {
          this.songs = songs;
        }
      });
  }

  sync() {
    this.songService.syncSongs()
      .subscribe({
        next: songs => {
          this.songs = songs;
        }
      });
  }
}
