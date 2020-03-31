import { Router, ActivatedRoute } from '@angular/router';
import { AlertifyService } from '../../_service/alertify.service';
import { UserService } from '../../_service/user.service';
import { Component, OnInit } from '@angular/core';
import { User } from '../../_models/user';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  users: User[];
  constructor(private userservice: UserService, private alertify: AlertifyService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.data.subscribe(data => {
        this.users = data.users;
    });    
    //this.loadUsers();
  }

  /*
  loadUsers(): void{
    this.userservice.getUsers().subscribe((users: User[]) =>
    {
      this.users = users;
    }, error =>{
      this.alertify.error(error);
    });
  }
  */
}
