import { AlertifyService } from './../../_service/alertify.service';
import { UserService } from './../../_service/user.service';
import { AuthService } from './../../_service/auth.service';
import { User } from './../../_models/user';
import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {

  @Input() user: User;

  constructor(private auservice: AuthService, private userservice: UserService, private alertify: AlertifyService) { }

  ngOnInit(): void {
  }

  sendLike(id: number){
    this.userservice.like(this.auservice.decodeToken.nameid, id).subscribe(data=>{
      this.alertify.success('You have like');
    }, error =>
    {
      this.alertify.error(error);
    }

    );
    
  }

}
