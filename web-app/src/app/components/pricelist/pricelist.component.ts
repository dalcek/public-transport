import { Component, OnInit } from '@angular/core';
import { TicketService } from 'src/app/services/ticket/ticket.service';

@Component({
  selector: 'app-pricelist',
  templateUrl: './pricelist.component.html',
  styleUrls: ['./pricelist.component.css']
})
export class PricelistComponent implements OnInit {

  prices: number[] = [];
  discounts: any[] = new Array(3);

  constructor(private ticketService: TicketService) { }

  ngOnInit(): void {
    this.fillTheTable();
  }

  fillTheTable() {
    this.ticketService.getAllPrices().subscribe(
      result => {
        this.prices = result.data;
        this.ticketService.getCoefficients().subscribe(
          res => {
            // Discounts - numbers
            for (let i = 0; i < 3; i++) {
              this.discounts[i] = res.data[i].value;
            }
            // Student prices
            for (let i = 4; i < 8; i++) {
              this.prices[i] = Math.round(this.discounts[1] * this.prices[i - 4]);
            }
            // Retired prices
            for (let i = 8; i < 12; i++) {
              this.prices[i] = Math.round(this.discounts[2] * this.prices[i - 8]);
            }
            // Discounts - string
            for (let i = 0; i < 3; i++) {
              this.discounts[i] = `${Math.round((1 - this.discounts[i]) * 100)}%`;
            }
          },
          error => {
            window.alert('Getting coefficients failed.');
          }
        );
      },
      error => {
        window.alert('Getting prices failed.');
      }
    );
  }
}
