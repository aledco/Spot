import { Component, OnInit } from "@angular/core";
import { PlaylistService } from "../services/playlist.service";
import { SimplifiedSpotifyPlaylist } from "../../core/interfaces/spotify/spotify-simplified-playlist.interface";
import { finalize } from "rxjs";
import { ToastrService } from "ngx-toastr";

@Component({
  selector: 'app-playlist-list',
  templateUrl: './playlist-list.component.html'
})
export class PlaylistListComponent implements OnInit {
  loaded: boolean = false;
  busy: boolean = false;
  playlists!: SimplifiedSpotifyPlaylist[];

  constructor(private playlistService: PlaylistService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.playlistService.getPlaylists()
      .pipe(finalize(() => this.loaded = true))
      .subscribe({
        next: playlists => {
          this.playlists = playlists;
        }
      });
  }

  shuffle(playlist: SimplifiedSpotifyPlaylist) {
    this.busy = true;
    this.playlistService.shufflePlaylist(playlist.id)
      .pipe(finalize(() => this.busy = false))
      .subscribe({
        next: _ => {
          this.toastr.success('Playlist was shuffled')
        }
      });
  }
}
