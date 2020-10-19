import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { PaymentDTO } from 'src/app/models/models';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {

  baseUrl = 'http://localhost:6003';

  constructor(private http: HttpClient) { }

  addPayment(payment: PaymentDTO): Observable<any> {
    return this.http.post<PaymentDTO>(`${this.baseUrl}/payment/addpayment`, payment)
      .pipe(catchError((err) => {
        console.log('Error in register service');
        console.error(err);
        return throwError(err)
      }));
  }
}
