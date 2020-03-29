import { AuthGuard } from './_guards/auth.guard';
import { HomeComponent } from './home/home.component';
import { Routes, CanActivate } from '@angular/router';
import { MemberListComponent } from './member-list/member-list.component';
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
            {path: 'members', component: MemberListComponent},
            {path: 'message', component: MessagesComponent},
        ]
    },
    {path: '**', redirectTo: '', pathMatch: 'full'} // full ???
]

