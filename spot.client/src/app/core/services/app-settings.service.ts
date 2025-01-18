import { Observable, Subject, of } from "rxjs";
import { AppSettings } from "../interfaces/configuration/app-settings.interface";
import { Injectable } from "@angular/core";

@Injectable()
export class AppSettingsService {
  private appSettingsSubject: Subject<AppSettings> = new Subject<AppSettings>();
  private storedAppSettings!: AppSettings;

  get appSettings(): Observable<AppSettings> {
    if (this.storedAppSettings) {
      return of(this.storedAppSettings);
    } else {
      return this.appSettingsSubject;
    }
  }

  set appSettings(appSettings: AppSettings) {
    this.storedAppSettings = appSettings;
    this.appSettingsSubject.next(appSettings);
  }
}
