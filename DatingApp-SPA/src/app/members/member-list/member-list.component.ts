import { UserService } from './../../_service/user.service';
import { Pagination, PaginatedResult } from './../../_models/pagination';
import { Router, ActivatedRoute } from '@angular/router';
import { AlertifyService } from '../../_service/alertify.service';
import { Component, OnInit } from '@angular/core';
import { User } from '../../_models/user';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  pagination: Pagination;
  users: User[];
  user: User = JSON.parse(localStorage.getItem('user'));
  genderList = [{ value: 'male', display: 'Male'}, { value: 'female', display: 'Female'}];
  userParam: any = {};
  constructor(private userservice: UserService, private alertify: AlertifyService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.data.subscribe(data => {
        this.users = data.users.result;
        this.pagination = data.users.pagination;
        console.log(this.pagination.itemsPerPage);
    });    
    //this.loadUsers();
    this.userParam.gender = (this.user.gender == 'male' ? 'female' : 'male');
    this.userParam.minAge = 18;
    this.userParam.maxAge = 99;
    this.userParam.orderBy = 'lastActived';
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    console.log(this.pagination);
    this.loadUsers();
  }

  resetFilters() {
    this.userParam.gender = this.user.gender === 'male' ? 'female' : 'male';
    this.userParam.minAge = 18;
    this.userParam.maxAge = 99;
    this.loadUsers();
  }
  
  loadUsers(): void{
    console.log(this.userParam);
    this.userservice.getUsers(this.pagination.currentPage, this.pagination.itemsPerPage, this.userParam).subscribe((users: PaginatedResult<User[]>) =>
    {
      this.users = users.result;
      this.pagination = users.pagination;
    }, error =>{
      this.alertify.error(error);
    });
  }
  
}
