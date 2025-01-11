import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SongRoutingModule } from './song-routing.module';
import { SongListComponent } from './song-list/song-list.component';
import { SongService } from './services/song.service';

;

@NgModule({
  declarations: [
    SongListComponent
  ],
  providers: [
    SongService
  ],
  imports: [
    CommonModule,
    SongRoutingModule
  ]
})
export class SongModule { }
