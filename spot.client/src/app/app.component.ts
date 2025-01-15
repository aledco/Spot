import { Component, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { AuthService } from './core/services/auth.service';
import { ConfigurationService } from './core/services/configuration.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {

  menuItems!: MenuItem[];

  constructor(private auth: AuthService, private config: ConfigurationService) {}

  get isLoggedIn(): boolean {
    return this.auth.isLoggedIn;
  }

  ngOnInit() {
    this.menuItems = this.buildMenuItems();

    this.config.getConfiguration().subscribe();
  }

  signout() {
    this.auth.signout();
  }

  private buildMenuItems(): MenuItem[] {
    return [
      {
        label: "Playlists",
        items: [
          {
            label: "List",
            routerLink: "/playlist/list"
          }
        ]
      },
      {
        label: "Songs",
        items: [
          {
            label: "List",
            routerLink: "/song/list"
          }
        ]
      },
      {
        label: "Song Tags",
        items: [
          {
            label: "List",
            routerLink: "/songtag/list"
          }
        ]
      }
    ];
  }
}
