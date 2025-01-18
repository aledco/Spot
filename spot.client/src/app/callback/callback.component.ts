import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { AuthService } from "../core/services/auth.service";

@Component({
  selector: 'app-callback',
  templateUrl: './callback.component.html'
})
export class CallbackComponent implements OnInit {
  constructor(private authService: AuthService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    const code = this.route.snapshot.queryParamMap.get('code');
    const state = this.route.snapshot.queryParamMap.get('state');
    if (code && state) {
      this.authService.postLogin(code, state);
    }
    else {
      this.authService.signout();
    }
  }
}
