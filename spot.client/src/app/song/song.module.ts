import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SongRoutingModule } from './song-routing.module';
import { SongListComponent } from './song-list/song-list.component';
import { SongService } from './services/song.service';
import { TableModule } from 'primeng/table';
import { MultiSelectModule } from 'primeng/multiselect';
import { InputTextModule } from 'primeng/inputtext';
import { FormsModule } from '@angular/forms';
import { EditSongComponent } from './edit-song/edit-song.component';
import { SongTagService } from '../song-tag/services/song-tag.service';

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
    CommonModule,
    FormsModule,
    TableModule,
    MultiSelectModule,
    InputTextModule,
    SongRoutingModule
  ]
})
export class SongModule { }
