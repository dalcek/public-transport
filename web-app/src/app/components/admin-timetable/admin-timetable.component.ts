import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Timestamp } from 'rxjs/internal/operators/timestamp';
import { AddDepartureDTO, DepartureDTO, LineNameDTO } from 'src/app/models/models';
import { RouteService } from 'src/app/services/route/route.service';

@Component({
  selector: 'app-admin-timetable',
  templateUrl: './admin-timetable.component.html',
  styleUrls: ['./admin-timetable.component.css']
})
export class AdminTimetableComponent implements OnInit {

   timetableForm = this.formBuilder.group({
      dayType: ['Weekday'],
      lineType: ['City'],
      lineName: ['Choose line'],
      departure: ['choose'],
      time: ['']
   });

   lines: LineNameDTO[];
   departures: DepartureDTO[];
   timetableId: number;
   departureId: number;
   lineId: number;

   constructor(private formBuilder: FormBuilder, private routeService: RouteService) { }

   ngOnInit(): void {
      this.getLines();
   }

   getLines() {
      this.routeService.getLineNames(this.timetableForm.controls.dayType.value, this.timetableForm.controls.lineType.value).subscribe(
         result => {
            this.lines = result.data;
            console.log(this.lines);
         },
         err => {
            console.log(err.error.message);
         }
      );
   }

   onLineTypeChange() {
      this.getLines();
      this.timetableForm.controls.lineName.setValue('Choose line');
      this.departures = [];
   }

   onDayTypeChange() {
      this.getLines();
      this.timetableForm.controls.lineName.setValue('Choose line');
      this.departures = [];
   }

   onLineChange(event: any) {
      console.log(event.target.value);
      if (event.target.value != 'Choose line')
      {
         this.lineId = parseInt(event.target.value);
         this.getDepartures();
      }
      else {
         this.departures = [];
      }
   }

   onDepartureChange(event: any) {
      if (event.target.value != 'choose' && event.target.value != 'add') {
         this.departureId = parseInt(event.target.value);
         for (let i = 0; i < this.departures.length; i++) {
            if (this.departures[i]['id'] == this.departureId) {
               this.timetableForm.controls.time.setValue(this.departures[i]['time']);
               break;
            }
         }
      }
   }

   getDepartures() {
      this.routeService.getDepartures(this.timetableForm.controls.dayType.value, this.lineId).subscribe(
         result => {
            this.departures = result.data.departures;
            this.timetableId = result.data.timetableId;
            this.timetableForm.controls.departure.setValue('choose');
            console.log(result);
         },
         err => {
            console.log(err.error.message);
         }
      );
   }

   addDeparture() {
      this.routeService.addDeparture(new AddDepartureDTO(0, this.timetableForm.controls.time.value, this.timetableId)).subscribe(
         result => {
            this.departures = result.data.departures;
            this.timetableForm.controls.departure.setValue('choose');
            this.timetableForm.controls.time.setValue('');
            console.log(result);
         },
         err => {
            console.log(err.error.message);
         }
      );
   }

   editDeparture() {
      let tmp = new AddDepartureDTO(this.departureId, this.timetableForm.controls.time.value, this.timetableId);
      this.routeService.editDeparture(tmp).subscribe(
         result => {
            this.departures = result.data.departures;
            this.timetableForm.controls.departure.setValue('choose');
            this.timetableForm.controls.time.setValue('');
         },
         err => {
            console.log(err.error.message);
         }
      );
   }

   deleteDeparture() {
      this.routeService.deleteDeparture(this.departureId).subscribe(
         result => {
            this.timetableForm.controls.departure.setValue('choose');
            this.timetableForm.controls.time.setValue('');
            this.getDepartures();
         },
         err => {
            console.log(err.error.message);
         }
      );
   }
}
