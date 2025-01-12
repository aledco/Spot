import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SongListComponent } from './song-list/song-list.component';
import { EditSongComponent } from './edit-song/edit-song.component';

const routes: Routes = [
  {
    path: 'list',
    component: SongListComponent
  },
  {
    path: 'edit/:songId',
    component: EditSongComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SongRoutingModule { }

