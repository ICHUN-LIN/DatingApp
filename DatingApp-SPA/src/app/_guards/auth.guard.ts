import { AlertifyService } from './../_service/alertify.service';
import { AuthService } from './../_service/auth.service';
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
}) // if it says 'root', not need to add to our modules to let others use
export class AuthGuard implements CanActivate {
  
  constructor(private authService: AuthService, private router: Router , private alertify: AlertifyService )
  {}
  
  canActivate(): boolean {

    if (this.authService.loggedin()) {
      return true;
    }

    this.alertify.message('You havnt logined yet. Accesss is not allowed!!');
    this.router.navigate(['/home']);

    return false;
  }
  
}
