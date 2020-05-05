import { PaginatedResult } from './../_models/pagination';
import { User } from './../_models/user';
import { Observable } from 'rxjs';

import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { environment } from './../../environments/environment';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';

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

  getUsers(page?, itemsperPage?, userParams?, likeParam?): Observable<PaginatedResult<User[]>> {
    //through this function, you can see what's get method need and return tye is decided by overseve parameter
    const paginatedResult: PaginatedResult<User[]> = new PaginatedResult<User[]>();
    let params = new HttpParams();

    if(page != null && itemsperPage != null) {
      params = params.append('pageNumbers', page);
      params = params.append('pageSize', itemsperPage);
    }

    if (likeParam == 'Likers'){
      params = params.append('likers', 'true');
    }

    if(likeParam == 'Likees'){
      params = params.append('likees','true');
    }


    if(userParams != null) {
      params = params.append('minAge', userParams.minAge);
      params = params.append('maxAge', userParams.maxAge);
      params = params.append('gender', userParams.gender);
      params = params.append('orderBy', userParams.orderBy);
    }

    //observe:'response' -> give all http reponse
    return this.http.get<User[]>(this.baseUrl + 'users', { observe: 'response', params }).pipe(
      map(response => {
        paginatedResult.result = response.body;
        if(response.headers.get('Pagination') != null){
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'))
        }
        return paginatedResult;
      })
    );
  }

  getUser(id): Observable<User>{
    return this.http.get<User>(this.baseUrl + 'users/' + id);
  }

  updateUser(id: number, user: User): any {
    return this.http.put(this.baseUrl+ 'users/'+ id, user);
  }

  setMainPhoto(userid: number, id: number): any {
    return this.http.post(this.baseUrl+ 'users/' + userid + '/photos/' + id + '/setmain', {});
  }

  deletePhoto(userid: number, id: number): any {
    return this.http.delete(this.baseUrl + 'users/' + userid + '/photos/' + id);
  }

  like(userid: number, recipientId: number): any {
    return this.http.post(this.baseUrl + 'users/' + userid + '/like/'+ recipientId,{});
  }
}
