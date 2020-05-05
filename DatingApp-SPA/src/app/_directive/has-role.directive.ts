import { AuthService } from './../_service/auth.service';
import { Directive, Input, ViewContainerRef, TemplateRef, OnInit } from '@angular/core';

@Directive({
  selector: '[appHasRole]'
})
export class HasRoleDirective implements OnInit {
  @Input() appHasRole: string[];
  // show rage or not
  isVisible = false;
  constructor(
    private viewContainerRefr: ViewContainerRef,
    private templateRef: TemplateRef<any>,
    private authService: AuthService) {

    }
  ngOnInit(): void {

    var roles = this.authService.getDecodeToken().role as Array<string>;
    // if no roles in user, clear the viewContainer
    if( !roles ){
       this.viewContainerRefr.clear();
    }


    // if user's role macth => render roles
    if( this.authService.roleMatch(this.appHasRole) ) {
      if( !this.isVisible){
        this.isVisible = true;
         //if match => create orignal ref
        this.viewContainerRefr.createEmbeddedView(this.templateRef);
      }
    } else {
       this.isVisible = false;
       this.viewContainerRefr.clear();
    }

  }

    //OnInit

}
