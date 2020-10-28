import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { TicketService } from 'src/app/services/ticket/ticket.service';

@Component({
  selector: 'app-controller-ticket',
  templateUrl: './controller-ticket.component.html',
  styleUrls: ['./controller-ticket.component.css']
})
export class ControllerTicketComponent implements OnInit {

   validationForm = this.formBuilder.group({
      id: ['', Validators.required],
   });

   message: string;
   id: number;
   constructor(private formBuilder: FormBuilder, private ticketService: TicketService) { }

   ngOnInit(): void {
   }

   validateTicket() {
      this.ticketService.validateTicket(this.validationForm.controls.id.value).subscribe(
         result => {
            this.id = this.validationForm.controls.id.value;
            if (result.data) {
               this.message = `Ticket with id ${this.id} is valid.`;
            }
            else {
               this.message = `Ticket with id ${this.id} is invalid.`;
            }
         },
         err => {
            console.log(err.error.message);
         }
      );
   }

   validateForm(): boolean{
      if (this.validationForm.controls.id.errors) {
        return false;
      }
      return true;
   }
}
