import { NgModule } from '@angular/core';
import { PlaylistRoutingModule } from './playlist-routing.module';
import { PlaylistListComponent } from './playlist-list/playlist-list.component';
import { PlaylistService } from './services/playlist.service';
import { CommonModule } from '@angular/common';

;

@NgModule({
  declarations: [
    PlaylistListComponent
  ],
  providers: [
    PlaylistService
  ],
  imports: [
    CommonModule,
    PlaylistRoutingModule
  ]
})
export class PlaylistModule { }
