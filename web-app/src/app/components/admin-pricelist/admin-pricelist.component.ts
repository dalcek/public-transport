import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { PricelistDTO } from 'src/app/models/models';
import { TicketService } from 'src/app/services/ticket/ticket.service';

@Component({
  selector: 'app-admin-pricelist',
  templateUrl: './admin-pricelist.component.html',
  styleUrls: ['./admin-pricelist.component.css']
})
export class AdminPricelistComponent implements OnInit {

  editPricelistForm = this.formBuilder.group({
    from: ['', Validators.required],
    to: ['', Validators.required],
    hourPrice: ['', Validators.required],
    dayPrice: ['', Validators.required],
    monthPrice: ['', Validators.required],
    yearPrice: ['', Validators.required],
  });

  createPricelistForm = this.formBuilder.group({
    from: ['', Validators.required],
    to: ['', Validators.required],
    hourPrice: ['', Validators.required],
    dayPrice: ['', Validators.required],
    monthPrice: ['', Validators.required],
    yearPrice: ['', Validators.required],
  });

  tab: string = '#nav-edit';

  constructor(private formBuilder: FormBuilder, private ticketService: TicketService) { }

  ngOnInit(): void {
    this.getPricelist();
  }

  getPricelist() {
    this.ticketService.getPricelist().subscribe(
      result => {
        console.log(result)
        let fullDate = result.data.from.split(' ')[0].split('/');
        let formattedDate = `${fullDate[2]}-${fullDate[0]}-${fullDate[1]}`;
        this.editPricelistForm.controls.from.setValue(formattedDate);

        fullDate = result.data.to.split(' ')[0].split('/');
        formattedDate = `${fullDate[2]}-${fullDate[0]}-${fullDate[1]}`;
        this.editPricelistForm.controls.to.setValue(formattedDate);

        this.editPricelistForm.controls.hourPrice.setValue(result.data.hourPrice);
        this.editPricelistForm.controls.dayPrice.setValue(result.data.dayPrice);
        this.editPricelistForm.controls.monthPrice.setValue(result.data.monthPrice);
        this.editPricelistForm.controls.yearPrice.setValue(result.data.yearPrice);
      },
      err => {
        console.log(err.error.message);
      }
    );
  }

  editPricelist() {
    this.ticketService.editPricelist(new PricelistDTO(this.editPricelistForm.controls.from.value, this.editPricelistForm.controls.to.value,
      this.editPricelistForm.controls.hourPrice.value, this.editPricelistForm.controls.dayPrice.value,
      this.editPricelistForm.controls.monthPrice.value, this.editPricelistForm.controls.yearPrice.value)).subscribe(
        result => {
          window.alert("Pricelist edit successful.");
        },
        err => {
          console.log(err.error.message);
          window.alert("Pricelist edit failed.");
        }
      );
  }

  createPricelist() {
    this.tab = '#nav-new'
  }
}
