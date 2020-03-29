import { AlertifyService } from './../_service/alertify.service';
import { AuthService } from './../_service/auth.service';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Input() valuesFromHome: any;
  //@Output() outputevent: new EventEmitter(); // new class
  @Output() outputevent = new EventEmitter(); //new class
  model: any = {};

  constructor(private authService: AuthService, private alertifyService: AlertifyService) { }

  ngOnInit(): void {
  }

  register(): void {
    console.log(this.model);
    this.authService.register(this.model).subscribe(() => {
        this.alertifyService.success('successfully register');
    }, error =>
    {
      this.alertifyService.error(error);
    }
    

    );
  }

  cancel(): void {
    //console.log('cancel');
    this.outputevent.emit(false);
    //console.log('end');
  }
}
