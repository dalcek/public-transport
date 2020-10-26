import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { DepartureDTO, LineNameDTO } from 'src/app/models/models';
import { RouteService } from 'src/app/services/route/route.service';

@Component({
  selector: 'app-timetable',
  templateUrl: './timetable.component.html',
  styleUrls: ['./timetable.component.css']
})
export class TimetableComponent implements OnInit {

   timetableForm = this.formBuilder.group({
      dayType: ['Weekday'],
      lineType: ['City'],
      lineName: ['Choose line'],
   });

   lines: LineNameDTO[];
   departures: DepartureDTO[];

   constructor(private formBuilder: FormBuilder, private routeService: RouteService) { }

   ngOnInit(): void {
      this.getLines();      
   }

   getLines() {
      this.routeService.getLines(this.timetableForm.controls.dayType.value, this.timetableForm.controls.lineType.value).subscribe(
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
         this.routeService.getDepartires(this.timetableForm.controls.dayType.value, this.timetableForm.controls.lineType.value, event.target.value).subscribe(
            result => {
               this.departures = result.data.departures;
               console.log(this.departures);
            },
            err => {
               console.log(err.error.message);
            }
         );
      }
      else {
         this.departures = [];
      }
   }
}
