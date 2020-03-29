
import { AlertifyService } from './_service/alertify.service';
import { ErrorInterCeptorProvider } from './_service/error.interceptor';
import { AuthService } from './_service/auth.service';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';


// RECOMMENDED
// import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { MemberListComponent } from './member-list/member-list.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { AppRoutsModel } from './rootes';
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    MemberListComponent,
    ListsComponent,
    MessagesComponent
  ],
  imports: [
    BrowserAnimationsModule,
    BsDropdownModule.forRoot(),
    BrowserModule,
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot(AppRoutsModel)
     // ngx boottraps give function of bottrap without jquery.js
  ],
  providers: [AuthService, ErrorInterCeptorProvider, AlertifyService ], //給indenpenncy injection 用
  bootstrap: [AppComponent] // 由此開始render
})
export class AppModule { }
