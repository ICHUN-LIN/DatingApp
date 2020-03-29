import { Injectable } from '@angular/core';
import * as alertify from 'alertifyjs';

@Injectable({
  providedIn: 'root'
})
export class AlertifyService {

  constructor() { }

  comfirm(msg: string, okcallback: () => any ): void {
    alertify.comfirm(msg, (e: any ) => {
      if ( e ) {
        okcallback();
      } else {}
    });
  }

  success(msg: string): void {
    alertify.success(msg);
  }

  error(msg: string): void {
    alertify.error(msg);
  }

  warning(msg: string): void {
    alertify.warning(msg);
  }

  message(msg: string): void {
    alertify.message(msg);
  }
}
