import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { LoginUserDTO } from '../models/models';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs/internal/observable/of';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseUrl = 'http://localhost:5000';

  constructor(private http: HttpClient) { }

  login (user: LoginUserDTO): Observable<LoginUserDTO> {
    let headers = new HttpHeaders();
    headers = headers.append('Content-type','application/x-www-fore-urlencoded');
    return this.http.post<LoginUserDTO>(`${this.baseUrl}/account/login`, user)
      .pipe(catchError(this.handleError<any>('login'))) as Observable<any>;
  }

  private handleError<T> (operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      return of(result as T);
    };
  }
}
