import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CallbackComponent } from './callback/callback.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { StorageService } from './core/services/storage.service';
import { AuthService } from './core/services/auth.service';
import { HomeComponent } from './home/home.component';
;

@NgModule({
  declarations: [
    AppComponent,
    CallbackComponent,
    DashboardComponent,
    HomeComponent
  ],
  providers: [
    StorageService,
    AuthService
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
