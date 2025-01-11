import { Component, OnInit } from "@angular/core";
import { PlaylistService } from "../services/playlist.service";

@Component({
  selector: 'app-playlist-list',
  templateUrl: './playlist-list.component.html'
})
export class PlaylistListComponent implements OnInit {

  playlistsResult!: any;

  constructor(private playlistService: PlaylistService) { }

  ngOnInit(): void {
    this.playlistService.getPlaylists()
      .subscribe({
        next: result => {
          this.playlistsResult = result;
        }
      });
  }
}
