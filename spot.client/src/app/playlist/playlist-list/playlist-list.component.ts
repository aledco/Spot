import { Component, OnInit } from "@angular/core";
import { PlaylistService } from "../services/playlist.service";
import { SimplifiedSpotifyPlaylist } from "../../core/interfaces/spotify/spotify-simplified-playlist.interface";

@Component({
  selector: 'app-playlist-list',
  templateUrl: './playlist-list.component.html'
})
export class PlaylistListComponent implements OnInit {

  playlists!: SimplifiedSpotifyPlaylist[];

  constructor(private playlistService: PlaylistService) { }

  ngOnInit(): void {
    this.playlistService.getPlaylists()
      .subscribe({
        next: playlists => {
          this.playlists = playlists;
        }
      });
  }
}
