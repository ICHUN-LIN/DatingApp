import { AlertifyService } from './../_service/alertify.service';
import { AuthService } from './../_service/auth.service';
import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot } from '@angular/router';

@Injectable({
  providedIn: 'root'
}) // if it says 'root', not need to add to our modules to let others use
export class AuthGuard implements CanActivate {
  
  constructor(private authService: AuthService, private router: Router , private alertify: AlertifyService ){

  }  
  canActivate(next: ActivatedRouteSnapshot): boolean {
    //protect first child(first level child), so get first child's data
    const roles = next.firstChild.data['roles'] as Array<string> ;
    if (roles) {
      const isMatch = this.authService.roleMatch(roles);
      if (isMatch) {
        return isMatch;
      } else {
        this.router.navigate(['members']);
        this.alertify.error('you are not allowed to access this area');
      }
    }
    
    if (this.authService.loggedin()) {
      return true;
    }

    this.alertify.message('You havnt logined yet. Accesss is not allowed!!');
    this.router.navigate(['/home']);

    return false;
  }
  
}
