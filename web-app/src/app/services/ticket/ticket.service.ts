import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class TicketService {

  baseUrl = 'http://localhost:6002';
  constructor(private http: HttpClient) { }

  getAllPrices(): Observable<any> {
    return this.http.get(`${this.baseUrl}/ticket/getallprices`)
    .pipe(catchError(err => {
      console.log('Error in get all prices service');
      console.error(err);
      return throwError(err);
    }));
  }

  getCoefficients(): Observable<any> {
    return this.http.get(`${this.baseUrl}/ticket/getcoefficients`)
    .pipe(catchError(err => {
      console.log('Error in get all prices service');
      console.error(err);
      return throwError(err);
    }));
  }
}
