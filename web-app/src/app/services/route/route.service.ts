import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AddDepartureDTO, AddLineDTO, AddStationDTO, LineDTO } from 'src/app/models/models';

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

   getLineNames(dayType: string, lineType: string): Observable<any> {
      return this.http.get(`${this.baseUrl}/route/getLineNames?dayType=${dayType}&lineType=${lineType}`)
      .pipe(catchError(err => {
         console.log('Error in get lines service');
         console.error(err);
         return throwError(err);
      }));
   }

   getDepartures(dayType: string, lineId: number): Observable<any> {
      return this.http.get(`${this.baseUrl}/route/getDepartures?dayType=${dayType}&lineId=${lineId}`)
      .pipe(catchError(err => {
         console.log('Error in get departures service');
         console.error(err);
         return throwError(err);
      }));
   }

   getStations(): Observable<any> {
      return this.http.get(`${this.baseUrl}/route/getStations`)
      .pipe(catchError(err => {
         console.log('Error in get stations service');
         console.error(err);
         return throwError(err);
      }));
   }

   getStationNames(): Observable<any> {
      return this.http.get(`${this.baseUrl}/route/getStationNames`)
      .pipe(catchError(err => {
         console.log('Error in get station names service');
         console.error(err);
         return throwError(err);
      }));
   }

   addStation(station: AddStationDTO): Observable<any> {
      return this.http.post(`${this.baseUrl}/route/addStation`, station)
      .pipe(catchError(err => {
         console.log('Error in add station service');
         console.error(err);
         return throwError(err);
      }));
   }

   editStation(station: AddStationDTO): Observable<any> {
      return this.http.put(`${this.baseUrl}/route/editStation`, station)
      .pipe(catchError(err => {
         console.log('Error in edit station service');
         console.error(err);
         return throwError(err);
      }));
   }

   deleteStation(id: number): Observable<any> {
      return this.http.delete(`${this.baseUrl}/route/deleteStation?id=${id}`)
      .pipe(catchError(err => {
         console.log('Error in delete station service');
         console.error(err);
         return throwError(err);
      }));
   }

   getLines(): Observable<any> {
      return this.http.get(`${this.baseUrl}/route/getLines`)
      .pipe(catchError(err => {
         console.log('Error in get lines service');
         console.error(err);
         return throwError(err);
      }));
   }

   getLineRoute(id: number): Observable<any> {
      return this.http.get(`${this.baseUrl}/route/getLineRoute?id=${id}`)
      .pipe(catchError(err => {
         console.log('Error in get line route service');
         console.error(err);
         return throwError(err);
      }));
   }

   addLine(line: AddLineDTO): Observable<any> {
      return this.http.post(`${this.baseUrl}/route/addLine`, line)
      .pipe(catchError(err => {
         console.log('Error in add line service');
         console.error(err);
         return throwError(err);
      }));
   }

   editLine(line: LineDTO): Observable<any> {
      return this.http.put(`${this.baseUrl}/route/editLine`, line)
      .pipe(catchError(err => {
         console.log('Error in edit line service');
         console.error(err);
         return throwError(err);
      }));
   }

   deleteLine(id: number): Observable<any> {
      return this.http.delete(`${this.baseUrl}/route/deleteLine?id=${id}`)
      .pipe(catchError(err => {
         console.log('Error in delete line service');
         console.error(err);
         return throwError(err);
      }));
   }
}
