import { Song } from "./song.interface";

export interface SongTag {
  id: number | null;
  name: string;
  descrpiption: string;
  isPublic: boolean;
  songTagCategory: any;
}
