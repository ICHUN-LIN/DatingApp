import { AuthService } from './_service/auth.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'app';
  constructor(private authService: AuthService) { }

  ngOnInit() {
    const user = JSON.parse(localStorage.getItem('user'));
    if ( user) {
      this.authService.currentUser = user;
      this.authService.changeMemberPhoto(this.authService.currentUser.photoUrl);
    }
  }
} // data it would provide to component.html
