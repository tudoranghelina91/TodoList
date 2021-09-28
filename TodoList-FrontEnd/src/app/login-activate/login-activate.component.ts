import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs";
import { LoginServiceService } from "../services/login-service.service";

@Injectable()
export class LoginActivate implements CanActivate {
  constructor(private authService: LoginServiceService, private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean>|Promise<boolean>|boolean {
    console.log('triggered');
    if (!this.authService.getLoginStatus()) {
      this.router.navigate(['login']);
    }
    return true;
  }
}