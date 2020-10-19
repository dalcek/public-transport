import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { CreateTicketDTO } from '../../models/models';

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

  getPrice(ticketType: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/ticket/getprice?ticketType=${ticketType}`)
    .pipe(catchError(err => {
      console.log('Error in get get price service');
      console.error(err);
      return throwError(err);
    }));
  }

  createTicket(ticket: CreateTicketDTO): Observable<any> {
    return this.http.post(`${this.baseUrl}/ticket/createticket`, ticket)
    .pipe(catchError(err => {
      console.log('Error in create ticket service');
      console.error(err);
      return throwError(err);
    }));
  }
}