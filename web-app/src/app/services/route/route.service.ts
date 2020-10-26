import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AddDepartureDTO } from 'src/app/models/models';

@Injectable({
  providedIn: 'root'
})
export class RouteService {

   baseUrl: string = 'http://localhost:5000';

   constructor(private http: HttpClient) { }

   addDeparture(departure: AddDepartureDTO): Observable<any> {
      return this.http.post(`${this.baseUrl}/route/addDeparture`, departure)
      .pipe(catchError(err => {
         console.log('Error in add departure service');
         console.error(err);
         return throwError(err);
      }));
   }

   editDeparture(departure: AddDepartureDTO): Observable<any> {
      console.log(departure);
      return this.http.put(`${this.baseUrl}/route/editDeparture`, departure)
      .pipe(catchError(err => {
         console.log('Error in edit departure service');
         console.error(err);
         return throwError(err);
      }));
   }

   deleteDeparture(id: number): Observable<any> {
      return this.http.delete(`${this.baseUrl}/route/deleteDeparture?id=${id}`)
      .pipe(catchError(err => {
         console.log('Error in edit departure service');
         console.error(err);
         return throwError(err);
      }));
   }

   getLines(dayType: string, lineType: string): Observable<any> {
      return this.http.get(`${this.baseUrl}/route/getLineNames?dayType=${dayType}&lineType=${lineType}`)
      .pipe(catchError(err => {
         console.log('Error in get lines service');
         console.error(err);
         return throwError(err);
      }));
   }

   getDepartures(dayType: string, lineType: string, lineId: number): Observable<any> {
      return this.http.get(`${this.baseUrl}/route/getDepartures?dayType=${dayType}&lineType=${lineType}&lineId=${lineId}`)
      .pipe(catchError(err => {
         console.log('Error in get lines service');
         console.error(err);
         return throwError(err);
      }));
   }
}
