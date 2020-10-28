import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AddDepartureDTO, CreateTicketDTO, PricelistDTO } from '../../models/models';

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
      console.log('Error in get price service');
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

  validateTicket(id: number): Observable<any> {
   return this.http.get(`${this.baseUrl}/ticket/validate?id=${id}`)
   .pipe(catchError(err => {
     console.log('Error in validate ticket service');
     console.error(err);
     return throwError(err);
   }));
 }

  getPricelist(): Observable<any> {
    return this.http.get(`${this.baseUrl}/ticket/getpricelist`)
    .pipe(catchError(err => {
      console.log('Error in get pricelist service');
      console.error(err);
      return throwError(err);
    }));
  }

  editPricelist(pricelist: PricelistDTO): Observable<any> {
    return this.http.put(`${this.baseUrl}/ticket/editpricelist`, pricelist)
    .pipe(catchError(err => {
      console.log('Error in edit pricelist service');
      console.error(err);
      return throwError(err);
    }));
  }

  createPricelist(pricelist: PricelistDTO): Observable<any> {
    return this.http.put(`${this.baseUrl}/ticket/createpricelist`, pricelist)
    .pipe(catchError(err => {
      console.log('Error in create pricelist service');
      console.error(err);
      return throwError(err);
    }));
  }
}


