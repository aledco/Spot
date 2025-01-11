import { SimplifiedSpotifyPlaylist } from "./spotify-simplified-playlist.interface";

export interface SpotifyPlaylistsResult {
  href: string;
  limit: number;
  next: string;
  offset: number;
  previous: string;
  total: number;
  items: SimplifiedSpotifyPlaylist[];
}
