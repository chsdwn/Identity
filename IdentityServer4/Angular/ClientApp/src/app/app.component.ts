import { Component } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";
import {
  OidcClientNotification,
  OidcSecurityService,
  PublicConfiguration,
} from "angular-auth-oidc-client";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
})
export class AppComponent {
  title = "Angular Client";

  constructor(
    public oidcSecurityService: OidcSecurityService,
    public http: HttpClient
  ) {}

  ngOnInit() {
    this.oidcSecurityService
      .checkAuth()
      .subscribe((auth) => console.log("is authenticated", auth));
  }

  login() {
    this.oidcSecurityService.authorize();
  }

  callApi() {
    const token = this.oidcSecurityService.getToken();

    const httpOptions = {
      headers: new HttpHeaders({
        Authorization: "Bearer " + token,
      }),
    };

    this.http
      .get("http://localhost:6000/secret", {
        headers: new HttpHeaders({
          Authorization: "Bearer " + token,
        }),
        responseType: "text",
      })
      .subscribe((data) => {
        console.log("api result:", data);
      });
  }
}
