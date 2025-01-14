import { Injectable } from "@angular/core";

@Injectable()
export class StorageService {

  store<T>(key: string, data: T): void {
    localStorage.setItem(key, JSON.stringify(data));
  }

  get<T>(key: string): T | null {
    const json = localStorage.getItem(key);
    if (json) {
      return JSON.parse(json) as T;
    }
    else {
      return null;
    }
  }

  delete(key: string): void {
    localStorage.removeItem(key);
  }

  has(key: string): boolean {
    const json = localStorage.getItem(key);
    if (json) {
      return true;
    }
    else {
      return false;
    }
  }
}
