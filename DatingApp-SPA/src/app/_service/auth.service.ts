import { environment } from './../../environments/environment';
import { logging } from 'protractor';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt'


@Injectable({
  providedIn: 'root' // means app mouules
})
export class AuthService {
  baseURL = environment.BaseUrl + 'auth/';
  jwtHelperService = new JwtHelperService(); //jwt encode decode
  decodeToken : any;

  constructor(private http: HttpClient) {  }

  login(model: any): any {
    // tslint:disable-next-line: no-unused-expression
    const result = this.http.post(this.baseURL + 'login', model).pipe(
      map((response: any) => {
        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);
          //this.decodeToken = this.jwtHelperService.decodeToken(user.token);
        }
      })
    );
    return result;
  }

  register(model: any): any {
    const result = this.http.post(this.baseURL + 'register', model);
    return result;
  }

  getDecodeToken(): any {
    if( !this.decodeToken) {
      const token = localStorage.getItem('token');
      if (token) { this.decodeToken = this.jwtHelperService.decodeToken(token)}
    }
    return this.decodeToken;
  }

  loggedin(): boolean {
    const token = localStorage.getItem('token');
    return !this.jwtHelperService.isTokenExpired(token);
  }

}
