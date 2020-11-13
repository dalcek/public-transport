import {HttpInterceptor,HttpRequest,HttpHandler,HttpEvent} from '@angular/common/http'
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
//import { AuthenticationService } from '../services/auth/authentication.service';

@Injectable()
export class TokenInterceptor implements HttpInterceptor{
   constructor (){}
   intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
      let jwt = localStorage.jwt;
      console.log(req);
      if(jwt){
         req = req.clone({
            setHeaders: {
               "Authorization": "Bearer "+ jwt
            }
         });
      }
      return next.handle(req);
   }
}
