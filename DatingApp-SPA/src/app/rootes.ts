import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { MemberEditResolver } from './_resolver/member-edit.resolver';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberListResolver } from './_resolver/member-list.resolver';
import { MemberDetailResolver } from './_resolver/member-detail.resolver';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { AuthGuard } from './_guards/auth.guard';
import { HomeComponent } from './home/home.component';
import { Routes, CanActivate } from '@angular/router';
import { MemberListComponent } from './members/member-list/member-list.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';


export const AppRoutsModel: Routes = [
    {path: '', component: HomeComponent},
    {
        //path:'dummy',//http:localhost:4200/dummy+....
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children:
        [
            {path: 'list', component: ListsComponent},
            //define route.data 中的name
            {path: 'members', component: MemberListComponent, 
                                resolve: {users: MemberListResolver}},
            {path: 'members/:id', component: MemberDetailComponent, 
                                resolve: {user: MemberDetailResolver}},
            {path: 'message', component: MessagesComponent},
            {path: 'member/edit', component: MemberEditComponent, resolve: {user: MemberEditResolver}, canDeactivate: [ PreventUnsavedChanges ]}
        ]
    },
    {path: '**', redirectTo: '', pathMatch: 'full'} // full ???
]

