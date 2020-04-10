import { Router } from '@angular/router';
import { User } from 'src/app/_models/user';
import { AlertifyService } from './../_service/alertify.service';
import { AuthService } from './../_service/auth.service';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker/public_api';

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
  user :User;
  registerForm: FormGroup;
  bsConfig: Partial<BsDatepickerConfig>;

  constructor(private authService: AuthService, private alertifyService: AlertifyService,
     private fb : FormBuilder, private router: Router) { }

  ngOnInit(): void {
    /*
    this.registerForm = new FormGroup({
        username: new FormControl('hi', Validators.required),
        password: new FormControl('',[ Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
        confirmPassword: new FormControl('', Validators.required)
      }, this.passMatchValidator
    );*/
    this.bsConfig = {
      containerClass: 'theme-red'
    };
    this.creatRegisterform();
  }

  creatRegisterform(): any {

    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: [''],
      dateOfBirth: [null, Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [ Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', Validators.required]
    }, {validators: this.passMatchValidator});
  }

  passMatchValidator(g: FormGroup): any{
    return g.get('password').value === g.get('confirmPassword').value ? null : { 'mismatch' : true};
  }

  register(): void {
    if ( this.registerForm.valid ) {
      //object.assign is copy back item to front item
      this.user = Object.assign({} , this.registerForm.value);
      //1. success 2. error 3. after complete
      this.authService.register(this.user).subscribe(() => {
        this.alertifyService.success('register success');
      }, error => { this.alertifyService.error(error)}, ()=>
      {
          this.authService.login(this.user).subscribe(()=>{
              this.router.navigate(['/members']);
          },error=>{
            this.alertifyService.error(error);
          });
      }
      );
    }
    /*
    console.log(this.model);
    this.authService.register(this.model).subscribe(() => {
        this.alertifyService.success('successfully register');
    }, error =>
    {
      this.alertifyService.error(error);
    }
    );*/
    console.log(this.registerForm.value);
  }

  cancel(): void {
    //console.log('cancel');
    this.outputevent.emit(false);
    //console.log('end');
  }
}
