import { SongTag } from "./song-tag.interface";

export interface Song {
  id: number | null;
  name: string;
  artist: string;
  spotifyId: string;
  tagIds:  number[];
  tags: string[];
}
