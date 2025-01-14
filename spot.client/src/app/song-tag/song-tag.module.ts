import { NgModule } from '@angular/core';
import { SongTagRoutingModule } from './song-tag-routing.module';
import { SongTagService } from './services/song-tag.service';
import { SongTagListComponent } from './song-tag-list/song-tag-list.component';
import { AddOrEditSongTagComponent } from './add-or-edit-song-tag/add-or-edit-song-tag.component';
import { CoreModule } from '../core/core.module';

@NgModule({
  declarations: [
    SongTagListComponent,
    AddOrEditSongTagComponent
  ],
  providers: [
    SongTagService
  ],
  imports: [
    CoreModule,
    SongTagRoutingModule
  ]
})
export class SongTagModule { }
