import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CallbackComponent } from './callback/callback.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { HomeComponent } from './home/home.component';
import { authGuard } from './core/guards/auth.guard';

const routes: Routes = [
  {
    path: 'home',
    component: HomeComponent
  },
  {
    path: 'callback',
    component: CallbackComponent
  },
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [authGuard]
  },
  {
    path: 'playlist',
    loadChildren: () => import('./playlist/playlist.module').then(m => m.PlaylistModule),
    canActivate: [authGuard]
  },
  {
    path: 'song',
    loadChildren: () => import('./song/song.module').then(m => m.SongModule),
    canActivate: [authGuard]
  },
  {
    path: 'songtag',
    loadChildren: () => import('./song-tag/song-tag.module').then(m => m.SongTagModule),
    canActivate: [authGuard]
  },
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full',
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}

