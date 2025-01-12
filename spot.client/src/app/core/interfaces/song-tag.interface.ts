import { Song } from "./song.interface";

export interface SongTag {
  id: number;
  name: string;
  songTagCategory: any;
  songs: Song[];
}
