import { NgModule } from '@angular/core';
import { PlaylistRoutingModule } from './playlist-routing.module';
import { PlaylistListComponent } from './playlist-list/playlist-list.component';
import { PlaylistService } from './services/playlist.service';
import { CoreModule } from '../core/core.module';

@NgModule({
  declarations: [
    PlaylistListComponent
  ],
  providers: [
    PlaylistService
  ],
  imports: [
    CoreModule,
    PlaylistRoutingModule
  ]
})
export class PlaylistModule { }
