import { AlertifyService } from './../../_service/alertify.service';
import { UserService } from './../../_service/user.service';
import { AuthService } from './../../_service/auth.service';
//import { environment } from './../../../environments/environment.prod ';
//remove prod means we want to use variable based on config(dev or pro)
import { environment } from './../../../environments/environment';
import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { Photo } from 'src/app/_models/photo';
import { FileUploader } from 'ng2-file-upload';
import { FileSelectDirective, FileDropDirective} from 'ng2-file-upload'


@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
  //directives: FileDropDirective, FileSelectDirective
})
export class PhotoEditorComponent implements OnInit {
  @Input() photos: Photo[];
  @Output() getMemberPhotoCange = new EventEmitter<string>();
  uploader: FileUploader;
  hasBaseDropZoneOver: boolean;
  hasAnotherDropZoneOver: boolean;
  response: string;
  baseUrl = environment.BaseUrl;
  currentMain: Photo;

  constructor(private authService: AuthService, private userService: UserService, private alertify: AlertifyService) { }

  ngOnInit(): void {
    this.initializeUploader();
  }

  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  public setMainPhoto(updatephoto: Photo): any {
    this.userService.setMainPhoto(this.authService.decodeToken.nameid, updatephoto.id).subscribe(() => {
      console.log('success set to main')
      this.currentMain = this.photos.filter(x => x.ismain === true)[0];
      this.currentMain.ismain = false;
      updatephoto.ismain = true;
      //this.getMemberPhotoCange.emit(updatephoto.url);
      this.authService.changeMemberPhoto(updatephoto.url);
      this.authService.currentUser.photoUrl = updatephoto.url;
      localStorage.setItem('user', JSON.stringify(this.authService.currentUser));
    }, error => {
      this.alertify.error(error);
    });
  }

  deletePhoto(id: number) : void {
    this.userService.deletePhoto(this.authService.decodeToken.nameid, id).subscribe(() => {
      //delete photo from array
      this.photos.splice(this.photos.findIndex(a => a.id == id), 1);
      this.alertify.success('photo has been deleted');
    }, error => {
      this.alertify.error(error);
    });
    /*
    this.alertify.comfirm('Are you sure you want to delete this photo?', () => {
      
      
    });*/
  }

  initializeUploader(): void {
    this.uploader = new FileUploader(
      {
        url: this.baseUrl + 'users/'+ this.authService.decodeToken.nameid + '/photos',
        authToken: 'Bearer ' + localStorage.getItem('token'),
        isHTML5: true,
        allowedFileType: ['image'],
        removeAfterUpload: true,
        autoUpload: false,
        maxFileSize: 10 * 1024 * 1024
      }
    );

    //problem fix -- > CORS policy: Response to preflight request doesn't pass access control check: The value of the 'Access-Control-Allow-Origin' header in the response must not be the wildcard '*' when the request's credentials mode is 'include'
    this.uploader.onAfterAddingFile = (file) => { file.withCredentials = false; };

    this.uploader.onSuccessItem = (item, response, status, header) => {
        if(response)
        {
          const res: Photo = JSON.parse(response);
          const photo = {
            id : res.id,
            url : res.url,
            dateAdded : res.dateAdded,
            description : res.description,
            ismain : res.ismain 
          };
          this.photos.push(photo);
        }
    };
  }

}
