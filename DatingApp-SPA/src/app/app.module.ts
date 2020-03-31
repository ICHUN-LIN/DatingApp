import { MemberDetailResolver } from './_resolver/member-detail.resolver';

import { AlertifyService } from './_service/alertify.service';
import { ErrorInterCeptorProvider } from './_service/error.interceptor';
import { AuthService } from './_service/auth.service';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { JwtModule} from '@auth0/angular-jwt';
import { NgxGalleryModule } from 'ngx-gallery-9';


// RECOMMENDED
// import { BsDropdownModule, TabsModule } from 'ngx-bootstrap/dropdown';
// import { BsDropdownModule, TabsModule } from 'ngx-bootstrap/dropdown';
import { BsDropdownModule} from 'ngx-bootstrap/dropdown';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { MemberListComponent } from './members/member-list/member-list.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { AppRoutsModel } from './rootes';
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberListResolver } from './_resolver/member-list.resolver';

export function tokengetter(){
  return localStorage.getItem('token');
}

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    MemberListComponent,
    ListsComponent,
    MessagesComponent,
    MemberCardComponent,
    MemberDetailComponent
  ],
  imports: [
    BrowserAnimationsModule,
    BsDropdownModule.forRoot(), // ngx boottraps give function of bottrap without jquery.js
    BrowserModule,
    HttpClientModule,
    FormsModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokengetter,
        whitelistedDomains:['localhost:5000'],
        blacklistedRoutes:['localhost:5000/api/auth']
      }
    }), //for get jwt token
    RouterModule.forRoot(AppRoutsModel),
    TabsModule.forRoot(),
    NgxGalleryModule // for gallery function     
  ],
  providers: [AuthService, ErrorInterCeptorProvider, AlertifyService,  MemberDetailResolver, MemberListResolver ], //給indenpenncy injection 用
  bootstrap: [AppComponent] // 由此開始render
})
export class AppModule { }
