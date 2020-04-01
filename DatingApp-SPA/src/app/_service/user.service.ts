import { User } from './../_models/user';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from './../../environments/environment';
import { Injectable } from '@angular/core';

/*
//Tempory use it
const HttpOPtions = {
  headers: new HttpHeaders({
    'Authorization' : 'Bearer '+ localStorage.getItem('token')
  })  
};
*/

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.BaseUrl;
  constructor(private http: HttpClient) { }

  getUsers(): Observable<User[]>{
    return this.http.get<User[]>(this.baseUrl + 'users');
  }

  getUser(id): Observable<User>{
    return this.http.get<User>(this.baseUrl + 'users/' + id);
  }

  updateUser(id: number, user: User): any {
    return this.http.put(this.baseUrl+ 'users/'+ id, user);
  }
}
