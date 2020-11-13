import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { PaymentDTO } from 'src/app/models/models';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {
  // For running on docker
  baseUrl = 'http://localhost:6003';
  // For running on k8s
  //baseUrl = 'http://localhost:80';

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
