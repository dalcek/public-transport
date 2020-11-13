import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { AddUserDTO, LoginUserDTO, ServiceResponse } from '../../models/models';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs/internal/observable/of';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
   // For running on docker
   baseUrl = 'http://localhost:6001';
   // For running on k8s
   //baseUrl = 'http://localhost:80';

  constructor(private http: HttpClient) { }

  login(user: LoginUserDTO): Observable<any> {
    let headers = new HttpHeaders();
    headers = headers.append('Content-type','application/x-www-fore-urlencoded');
    return this.http.post<LoginUserDTO>(`${this.baseUrl}/account/login`, user)
      .pipe(catchError((err) => {
        console.log('Error in login service');
        console.error(err);
        return throwError(err)
      }));
  }

  register(user: AddUserDTO): Observable<any> {
    return this.http.post<AddUserDTO>(`${this.baseUrl}/account/register`, user)
      .pipe(catchError((err) => {
        console.log('Error in register service');
        console.error(err);
        return throwError(err)
      }));
  }

  update(user: AddUserDTO): Observable<any> {
    return this.http.put<AddUserDTO>(`${this.baseUrl}/account/update`, user)
      .pipe(catchError((err) => {
        console.log('Error in update service');
        console.error(err);
        return throwError(err)
      }));
  }

   getUser(): Observable<any> {
     return this.http.get(`${this.baseUrl}/account`)
      .pipe(catchError((err) => {
         console.log('Error in get user service');
         console.error(err);
         return throwError(err)
      }));
   }

  getUnvalidatedUsers(): Observable<any> {
   return this.http.get(`${this.baseUrl}/account/getUnvalidatedUsers`)
     .pipe(catchError((err) => {
       console.log('Error in get unvalidated users service');
       console.error(err);
       return throwError(err)
     }));
  }
  validate(email: string, accepted: boolean): Observable<any> {
   return this.http.get(`${this.baseUrl}/account/validate?email=${email}&accepted=${accepted}`)
     .pipe(catchError((err) => {
       console.log('Error in validating user service');
       console.error(err);
       return throwError(err)
     }));
  }

  uploadImage(image: any): Observable<any>{
    return this.http.post(`${this.baseUrl}/account/uploadimage`, image)
    .pipe(catchError((err) => {
      console.log('Error in upload image service');
      console.error(err);
      return throwError(err)
    }));
  }

  deleteUser(user: AddUserDTO): Observable<any> {
   return this.http.post(`${this.baseUrl}/account/delete`, user)
    .pipe(catchError((err) => {
       console.log('Error in get user service');
       console.error(err);
       return throwError(err)
    }));
 }
}
