import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddOrEditSongTagComponent } from './add-or-edit-song-tag/add-or-edit-song-tag.component';
import { SongTagListComponent } from './song-tag-list/song-tag-list.component';

const routes: Routes = [
  {
    path: 'list',
    component: SongTagListComponent
  },
  {
    path: 'edit/:songTagId',
    component: AddOrEditSongTagComponent
  },
  {
    path: 'add',
    component: AddOrEditSongTagComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SongTagRoutingModule { }

