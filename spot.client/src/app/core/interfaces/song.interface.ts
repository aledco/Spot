import { SongTag } from "./song-tag.interface";

export interface Song {
  id: number;
  name: string;
  artist: string;
  spotifyId: string;
  tagIds:  number[];
  tags: string[];
}
