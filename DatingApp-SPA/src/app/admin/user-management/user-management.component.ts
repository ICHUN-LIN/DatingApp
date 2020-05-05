import { logging } from 'protractor';
import { RolesModalComponent } from './../roles-modal/roles-modal.component';
import { AdminService } from './../../_service/admin.service';
import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/_models/user';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit {

  users: User[];
  bsModalRef: BsModalRef;

  constructor(private adminService: AdminService, private modalService: BsModalService) { }

  ngOnInit(): void {
    this.getUsersWithRoles();
  }

  getUsersWithRoles() {
    this.adminService.getUsersWithRoles().subscribe((users: User[]) => {
      this.users = users;
    }, error => {
      console.log(error);
    });

  }

  editRolesModal(user: User){
    console.log(user);
    const initialState = {
      user,
      roles: this.getRolesByUser(user),
    };
    //console.log(initialState);
    this.bsModalRef = this.modalService.show(RolesModalComponent, {initialState});
    this.bsModalRef.content.updateSelectedRole.subscribe( (value) => {
       const rolesToupdate = {
          roleNames : [...value.filter(el => el.checked == true).map(el => el.name)]
       };
       console.log(rolesToupdate);
       if(rolesToupdate){
         this.adminService.updateUserRole(user, rolesToupdate).subscribe(()=>{
            user.roles = [...rolesToupdate.roleNames];
         }, error => {
            console.log(error);
         });
       }
    });
    
    //this.bsModalRef.content.closeBtnName = 'Close';
  }

  getRolesByUser(user): any {
    const roles = [] ;
    const userRoles = user.roles;
    console.log('*'+userRoles);
    const availabRoles: any = [
      {name: 'Admin', value: 'Admin'},
      {name: 'Moderator', value: 'Moderator'},
      {name: 'Member', value: 'Member'},
      {name: 'VIP', value: 'VIP'}
    ];

    for(let j = 0 ; j < availabRoles.length; j++ ) {
      let isMatch = false;
      //console.log(availabRoles[j]);
      for (let i=0 ; i< userRoles.length; i++) {
        //console.log(userRoles[i]+' ---i');
        if(availabRoles[j].name === userRoles[i]){
          isMatch = true;
          availabRoles[j].checked = true;
          roles.push(availabRoles[j]);
          break; 
        }
      }

      if(!isMatch){
        availabRoles[j].checked =false;
        roles.push(availabRoles[j]);
      }
    }

    return roles;

  }
}
