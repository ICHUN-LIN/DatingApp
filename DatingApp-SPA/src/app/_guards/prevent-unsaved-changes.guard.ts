import { MemberEditComponent } from './../members/member-edit/member-edit.component';
import { Injectable } from "@angular/core";
import { CanDeactivate } from '@angular/router';

@Injectable()
export class PreventUnsavedChanges implements CanDeactivate<MemberEditComponent>{
    //leave this url, go to other page, jump to alert 
    canDeactivate(component: MemberEditComponent){
        if ( component.editForm.dirty ) {
            return confirm('Are you sure to restore?');        }

        return true;
    }

}