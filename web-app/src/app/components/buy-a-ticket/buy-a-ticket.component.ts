import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ICreateOrderRequest, IPayPalConfig } from 'ngx-paypal';
import { CreateTicketDTO, PaymentDTO } from 'src/app/models/models';
import { PaymentService } from 'src/app/services/payment/payment.service';
import { TicketService } from 'src/app/services/ticket/ticket.service';

@Component({
  selector: 'app-buy-a-ticket',
  templateUrl: './buy-a-ticket.component.html',
  styleUrls: ['./buy-a-ticket.component.css']
})
export class BuyATicketComponent implements OnInit {

  public payPalConfig ? : IPayPalConfig;
  price: number;
  displayPrice: string;
  loggedIn: any = localStorage['role'];

  ticketId: number = -1;

  emailForm = this.formBuilder.group({
    email: ['', [Validators.required, Validators.email]],
    ticketType: ['HourTicket']
  });
  constructor(private formBuilder: FormBuilder, private ticketService: TicketService, private paymentService: PaymentService) { }

  ngOnInit(): void {
    if (this.loggedIn)
    {
      this.emailForm.controls.email.setValue(localStorage['email']);
    }
    this.getPrice();
    this.initConfig();
  }
  ticketTypeChanged(type: any){
    this.getPrice();
  }

  getPrice() {
    this.ticketService.getPrice(this.emailForm.controls.ticketType.value).subscribe(
      result => {
        console.log(result);
        this.price = result.data;
        this.displayPrice = `${this.price} RSD`;
      },
      err => {
        console.log(err.error.message);
        window.alert(err.error.message);
      }
    );
  }

  private initConfig(): void {
    this.payPalConfig = {
    currency: 'USD',
    clientId: 'AVfkLF27DVZa7MRFpAYUnJQ1jmpV3iF7D-cYlQM5n9EYvf_XNHyU_RxCYUqeYhhBnXNquku3sMq_0yI5',
    createOrderOnClient: (data) => <ICreateOrderRequest>{
      intent: 'CAPTURE',
      purchase_units: [
        {
          amount: {
            currency_code: 'USD',
            value: (this.price/100).toString(),
            breakdown: {
              item_total: {
                currency_code: 'USD',
                value: (this.price/100).toString()
              }
            }
          },
          payee: {
            email_address: 'sb-vj435n550635@personal.example.com'
          },
          items: [
            {
              name: 'Bus Ticket',
              quantity: '1',
              category: 'DIGITAL_GOODS',
              unit_amount: {
                currency_code: 'USD',
                value: (this.price/100).toString(),
              },
            }
          ]
        }
      ]
    },
    advanced: {
      commit: 'true'
    },
    style: {
        label: 'paypal',
        layout: 'horizontal',
        shape: 'pill'
      },

    onApprove: (data, actions) => {
      //console.log('onApprove - transaction was approved, but not authorized', data, actions);
      actions.order.get().then(details => {
        //console.log('onApprove - you can get full order details inside onApprove: ', details);
      });
    },
    onClientAuthorization: (data) => {
      //console.log('onClientAuthorization - you should probably inform your server about completed transaction at this point', data);
      this.ticketService.createTicket(new CreateTicketDTO(this.emailForm.controls.ticketType.value, this.emailForm.controls.email.value)).subscribe(
        result => {
          window.alert(`Succesfully purchased a ticket. Id: ${result.data.id}`);
          this.ticketId = result.data.id;
        },
        err => {
          console.log(err.error.message);
          window.alert(err.error.message);
          this.ticketId = -1;
        },
        () => {
          this.paymentService.addPayment(new PaymentDTO(data.id, data.payer.payer_id, data.payer.email_address, this.ticketId)).subscribe(res => console.log(res));
        }
      );
    },
    onCancel: (data, actions) => {
      //console.log('OnCancel', data, actions);
    },
    onError: err => {
      //console.log('OnError', err);
    },
    onClick: (data, actions) => {
      //console.log('onClick', data, actions);
    },
  };
  }
}
