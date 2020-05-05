import { User } from 'src/app/_models/user';
import { environment } from './../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.BaseUrl;
  constructor(private http: HttpClient) { }

  getUsersWithRoles(){
    return this.http.get(this.baseUrl + 'admin/usersWithRoles');
  }

  updateUserRole(user: User, roles: {}){
    return this.http.post(this.baseUrl + 'admin/editRoles/' +user.userName, roles);
  }
}
