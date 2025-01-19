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
import { provideAnimations } from '@angular/platform-browser/animations';
import { MenubarModule } from 'primeng/menubar';
import { CoreModule } from './core/core.module';
import { ToastrModule, provideToastr } from 'ngx-toastr';
import { ConfigurationService } from './core/services/configuration.service';
import { AppSettingsService } from './core/services/app-settings.service';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';

@NgModule({
  declarations: [
    AppComponent,
    CallbackComponent,
    DashboardComponent,
    HomeComponent
  ],
  providers: [
    StorageService,
    AuthService,
    AppSettingsService,
    ConfigurationService,
    provideAnimations(),
    provideToastr(),
    provideAnimationsAsync()
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    MenubarModule,
    CoreModule,
    ToastrModule.forRoot(),
    AppRoutingModule
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
