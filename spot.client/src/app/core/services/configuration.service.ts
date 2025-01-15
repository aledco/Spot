import { Injectable } from "@angular/core";
import { BaseAPIService } from "../base/base-api.service";
import { Observable, Subject, map, tap } from "rxjs";
import { AppSettings } from "../interfaces/configuration/app-settings.interface";

@Injectable()
export class ConfigurationService extends BaseAPIService {

  appSettings: Subject<AppSettings> = new Subject<AppSettings>(); // config service cannot be injected in auth service as it cuases circular dependency

  getConfiguration(): Observable<AppSettings> {
    return this.get<AppSettings>("/configuration")
      .pipe(tap(appSettings => this.appSettings.next(appSettings)));
  }
}
