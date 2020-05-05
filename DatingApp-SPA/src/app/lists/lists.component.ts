import { AlertifyService } from './../_service/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { UserService } from './../_service/user.service';
import { AuthService } from './../_service/auth.service';
import { Pagination } from './../_models/pagination';
import { User } from './../_models/user';
import { Component, OnInit } from '@angular/core';
import { PaginatedResult } from './../_models/pagination';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  users: User[];
  pagination: Pagination;
  likesParam: string;

  constructor(private auservice: AuthService, private userservice: UserService, 
    private route: ActivatedRoute, private alertify: AlertifyService) { }

  ngOnInit(): void {
    this.route.data.subscribe(data => {
      this.users = data['users'].result;
      this.pagination = data['users'].pagination;
    });
    this.likesParam = 'Likers';
  }

  loadUsers(): void{
    //console.log(this.userParam);
    this.userservice.getUsers(this.pagination.currentPage, this.pagination.itemsPerPage, null,  this.likesParam).subscribe((users: PaginatedResult<User[]>) =>
    {
      this.users = users.result;
      this.pagination = users.pagination;
    }, error =>{
      this.alertify.error(error);
    });
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    console.log(this.pagination);
    this.loadUsers();
  }

}
