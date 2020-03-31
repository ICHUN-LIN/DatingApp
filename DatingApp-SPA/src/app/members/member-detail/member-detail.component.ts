import { AlertifyService } from './../../_service/alertify.service';
import { UserService } from './../../_service/user.service';
import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/_models/user';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryOptions } from 'ngx-gallery-9/lib/ngx-gallery-options';
import { NgxGalleryImage } from 'ngx-gallery-9/lib/ngx-gallery-image.model';
//import { NgxGalleryAnimation } from 'ngx-gallery-9/lib/ngx-gallery-animation.model';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {

  user: User;
  gallaryoption: NgxGalleryOptions[];
  gallaryImg: NgxGalleryImage[];

  constructor(private userservice: UserService, private alertify: AlertifyService, private router: ActivatedRoute) { }

  ngOnInit(): void {
    // get data from resolver in router 
    this.router.data.subscribe( data => {
      this.user = data.user;
      });

    this.gallaryoption =[{
          // square
          width: '500px',
          height: '500px',
          imagePercent: 100,
          thumbnailsColumns: 4,
          // ? 去哪了
          //imageAnimation: NgxGalleryAnimation,
          preview: false
        }
      ];

    this.gallaryImg = this.getImges();
  }

  getImges(): any {
    const imagsUrl = [];
    for ( const photo of this.user.photos){
      imagsUrl.push(
        {
          small: photo.url,
          medium: photo.url,
          big: photo.url,
          description: photo.description
        }
      );
    }
    return imagsUrl;
  }

  /*
  loaduser(): void {
    //members/4 (use router to get 4 paramter)
    //+ make it become number
    this.userservice.getUser(+this.router.snapshot.params['id']).subscribe(
      (user: User)=>{
        this.user = user;
      }, error => {
        this.alertify.error(error);
      }
      
    );
  }
  */

}
