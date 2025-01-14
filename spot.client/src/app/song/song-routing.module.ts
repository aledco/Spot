import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SongListComponent } from './song-list/song-list.component';
import { EditSongComponent } from './edit-song/edit-song.component';
import { AddSongComponent } from './add-song/add-song.component';

const routes: Routes = [
  {
    path: 'list',
    component: SongListComponent
  },
  {
    path: 'edit/:songId',
    component: EditSongComponent
  },
  {
    path: 'add',
    component: AddSongComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SongRoutingModule { }

