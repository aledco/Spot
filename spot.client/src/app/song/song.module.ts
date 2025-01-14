import { NgModule } from '@angular/core';
import { SongRoutingModule } from './song-routing.module';
import { SongListComponent } from './song-list/song-list.component';
import { SongService } from './services/song.service';
import { EditSongComponent } from './edit-song/edit-song.component';
import { SongTagService } from '../song-tag/services/song-tag.service';
import { CoreModule } from '../core/core.module';

@NgModule({
  declarations: [
    SongListComponent,
    EditSongComponent
  ],
  providers: [
    SongService,
    SongTagService
  ],
  imports: [
    CoreModule,
    SongRoutingModule
  ]
})
export class SongModule { }
