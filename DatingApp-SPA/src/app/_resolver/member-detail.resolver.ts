import { catchError } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { AlertifyService } from './../_service/alertify.service';
import { UserService } from './../_service/user.service';
import { User } from 'src/app/_models/user';
import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';

@Injectable()
export class MemberDetailResolver implements Resolve<User>
{
    constructor(private userservice: UserService, private alertify: AlertifyService, private router: Router){

    }

    resolve(route: ActivatedRouteSnapshot): Observable<User>{

        // tslint:disable-next-line: no-string-literal
        //put here, getUser is already subscribed automatically
        return this.userservice.getUser(route.params['id']).pipe(
            catchError( error =>{
                    this.alertify.error('problem retriving data');
                    this.router.navigate(['/members']);
                    return of(null);
                })
        );
    }
}