import { AlertifyService } from './../_service/alertify.service';
import { AuthService } from './../_service/auth.service';
import { logging } from 'protractor';
import { Component, OnInit } from '@angular/core';
import { tokenReference } from '@angular/compiler';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  photoUrl: string;

  constructor( public authService: AuthService, private alertifyService : AlertifyService,
    private router : Router) { 

  }

  ngOnInit(): void {
    this.authService.currentPhotoUrl.subscribe(url => this.photoUrl = url );
  }

  login(): void  {
    this.authService.login(this.model).subscribe(() => {
      this.alertifyService.success('login successfully');
      }, error => {
        this.alertifyService.error(error);
      }, complete =>
      {
        this.router.navigate(['/members']);
      });

    console.log(this.model);
  }

  // tslint:disable-next-line: align
  logedIn(): boolean {
    //const token = localStorage.getItem('token');
    //return !!token; // if token exist, return true, else return false
    return this.authService.loggedin();
  }

  logout(): void {
    localStorage.removeItem('token');
    this.authService.decodeToken = null;
    localStorage.removeItem('user');
    this.authService.currentUser = null;
    this.alertifyService.message('logout succefully');
    this.router.navigate(['/home']);
  }

}
