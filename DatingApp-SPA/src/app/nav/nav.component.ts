import { AuthService } from './../_service/auth.service';
import { logging } from 'protractor';
import { Component, OnInit } from '@angular/core';
import { tokenReference } from '@angular/compiler';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};

  constructor( private authService: AuthService) { }

  ngOnInit(): void {
  }

  login(): void  {
    this.authService.login(this.model).subscribe(() => {
        console.log('login successfully');
      }, error => {
        console.log(error);
      });

    console.log(this.model);
  }

  // tslint:disable-next-line: align
  logedIn(): boolean {
    const token = localStorage.getItem('token');
    return !!token; // if token exist, return true, else return false
  }

  logout(): void {
    localStorage.removeItem('token');
    console.log('logout succefully');
  }

}
