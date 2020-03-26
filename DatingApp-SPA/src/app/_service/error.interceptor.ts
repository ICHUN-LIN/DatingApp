import { Injectable } from '@angular/core'; // means this is injectable
import { HttpInterceptor, HttpErrorResponse, HTTP_INTERCEPTORS} from '@angular/common/http';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

type NewType = import('rxjs').Observable<import('@angular/common/http').HttpEvent<any>>;

@Injectable()
export class ErrorInterCeptor implements HttpInterceptor {
    // tslint:disable-next-line: max-line-length
    intercept(req: import('@angular/common/http').HttpRequest<any>, next: import('@angular/common/http').HttpHandler): NewType {
        return next.handle(req).pipe(
            catchError(error => {

                // error is the reponse we got from server
                if (error.status === 401) {
                    return throwError(error.statusText);
                }

                if (error instanceof HttpErrorResponse) {
                    // tslint:disable-next-line: variable-name
                    const app_error = error.headers.get('App-Error');
                    if (app_error) {
                        return throwError(app_error);
                    }

                    const Servererror = error.error;
                    let statusErrorMsg = '';
                    if (Servererror.errors && typeof Servererror.errors === 'object') {
                        // tslint:disable-next-line: forin
                        for (const key in Servererror.errors) {
                            statusErrorMsg += key + ':' + Servererror.errors[key] + '\n';

                        }
                    }
                    return throwError(statusErrorMsg || Servererror || 'unknow error!');
                }
            })
        );
    }
}

export const ErrorInterCeptorProvider = {
        provide: HTTP_INTERCEPTORS,
        useClass: ErrorInterCeptor,
        multi: true
    };
