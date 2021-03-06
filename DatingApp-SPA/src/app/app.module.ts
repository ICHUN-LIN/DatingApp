import { AdminService } from './_service/admin.service';
import { ListsResolver } from './_resolver/list.resolver';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { MemberEditResolver } from './_resolver/member-edit.resolver';
import { MemberDetailResolver } from './_resolver/member-detail.resolver';
//import {TimeAgoPipe} from 'time-ago-pipe';

import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { JwtModule} from '@auth0/angular-jwt';
import { NgxGalleryModule } from 'ngx-gallery-9';
import { BsDropdownModule} from 'ngx-bootstrap/dropdown';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ModalModule } from 'ngx-bootstrap/modal';
import { FileUploadModule } from 'ng2-file-upload';
// RECOMMENDED
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { PaginationModule } from 'ngx-bootstrap/pagination';


// RECOMMENDED
// import { BsDropdownModule, TabsModule } from 'ngx-bootstrap/dropdown';
// import { BsDropdownModule, TabsModule } from 'ngx-bootstrap/dropdown';
import { AlertifyService } from './_service/alertify.service';
import { ErrorInterCeptorProvider } from './_service/error.interceptor';
import { AuthService } from './_service/auth.service';
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
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { PhotoEditorComponent } from './members/photo-editor/photo-editor.component';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { HasRoleDirective } from './_directive/has-role.directive';
import { PhotoManagementComponent } from './admin/photo-management/photo-management.component';
import { UserManagementComponent } from './admin/user-management/user-management.component';
import { RolesModalComponent } from './admin/roles-modal/roles-modal.component';

export function tokengetter(){
  return localStorage.getItem('token');
}

@NgModule({
  declarations: [
   // FileDropDirective, FileSelectDirective,
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    MemberListComponent,
    ListsComponent,
    MessagesComponent,
    MemberCardComponent,
    MemberDetailComponent,
    MemberEditComponent,
    PhotoEditorComponent,
    AdminPanelComponent,
    HasRoleDirective,
    PhotoManagementComponent,
    UserManagementComponent,
    RolesModalComponent//,
    //FileSelectDirective
  ],
  imports: [
    //FileSelectDirective,
    BrowserAnimationsModule,
    BsDropdownModule.forRoot(), // ngx boottraps give function of bottrap without jquery.js
    BrowserModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    ButtonsModule.forRoot(),
    JwtModule.forRoot({
      config: {
        tokenGetter: tokengetter,
        whitelistedDomains:['localhost:5000'],
        blacklistedRoutes:['localhost:5000/api/auth']
      }
    }), //for get jwt token
    RouterModule.forRoot(AppRoutsModel),
    TabsModule.forRoot(),
    NgxGalleryModule,
    FileUploadModule,
    BsDatepickerModule.forRoot(),
    PaginationModule.forRoot(),
    ModalModule.forRoot()
    //,TimeAgoPipe  
  ],
  providers: [AuthService, ErrorInterCeptorProvider, AlertifyService,  MemberDetailResolver, MemberListResolver,
  MemberEditResolver, PreventUnsavedChanges, ListsResolver,AdminService ], //給indenpenncy injection 用
  entryComponents: [ RolesModalComponent ],
  bootstrap: [AppComponent] // 由此開始render

})
export class AppModule { }
