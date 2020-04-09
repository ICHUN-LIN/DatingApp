import { Photo } from './../../_models/photo';
import { AuthService } from './../../_service/auth.service';
import { UserService } from './../../_service/user.service';
import { AlertifyService } from './../../_service/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { User } from './../../_models/user';
import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm', { static: true}) editForm: NgForm;
  user: User;
  photoUrl: string;
  constructor(private authservice: AuthService, private userservice: UserService, private route: ActivatedRoute, private alertify: AlertifyService) { }

  ngOnInit(): void {
    this.route.data.subscribe(data => {
      this.user = data.user;
      }
    );
    this.authservice.currentPhotoUrl.subscribe(url => this.photoUrl = url);
  }

  updateUser(): void{
    //this.alertify.message('test updateUser');
    this.userservice.updateUser(this.authservice.decodeToken.nameid,this.user).subscribe
    ( next => {
        console.log(this.user);
        this.editForm.reset(this.user);
        this.alertify.success('successfully update');
      }, error => {
        this.alertify.error(error);
      }
    );


  }

  updateMainPhoto(photourl){
    this.user.photoUrl = photourl;
  }

}
