import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root' // means app mouules
})
export class AuthService {
  baseURL = 'http://localhost:5000/api/auth/';
  constructor(private http: HttpClient) { }

  login(model: any): any {
    // tslint:disable-next-line: no-unused-expression
    const result = this.http.post(this.baseURL + 'login', model).pipe(
      map((response: any) => {
        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);
        }
      })
    );
    return result;
  }

  register(model: any): any {
    const result = this.http.post(this.baseURL + 'register', model);
    return result;
  }

}
