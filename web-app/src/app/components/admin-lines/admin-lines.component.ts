import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { LineDTO, StationDTO } from 'src/app/models/models';
import { RouteService } from 'src/app/services/route/route.service';

@Component({
  selector: 'app-admin-lines',
  templateUrl: './admin-lines.component.html',
  styleUrls: ['./admin-lines.component.css']
})
export class AdminLinesComponent implements OnInit {

   lineForm = this.formBuilder.group({
      select: ['choose'],
      name: ['', Validators.required],
      type: ['', Validators.required]
   });

   stations: StationDTO[];
   lines: LineDTO[];
   lineId: number;
   stationsCheck: boolean[];
   indexes: number[];
   lineStationIds: number[] = [];
 
   constructor(private formBuilder: FormBuilder, private routeService: RouteService) { }

   ngOnInit(): void {
      this.getLines();
      this.getStations();
   }

   getLines() {
      this.routeService.getLines().subscribe(
         result => {
            console.log(result);
            this.lines = result.data;
         },
         err => {
            console.log(err.error.message);
         }
      );
   }

   getStations() {
      this.routeService.getStationNames().subscribe(
         result => {
            this.stations = result.data;
            this.indexes = [];
            for (let i = 0; i < this.stations.length; i++) {
               this.indexes.push(i);
            }
            this.stationsCheck = [];
            for (let i = 0; i < this.stations.length; i++) {
               this.stationsCheck.push(false);
            }
         },
         err => {
            console.log(err.error.message);
         }
      );
   }

   editLine() {
      this.lineStationIds = [];
      for (let i = 0; i < this.stations.length; i++) {
         if (this.stationsCheck[i]) {
            this.lineStationIds.push(this.stations[i]['id']);
         }
      }
      this.routeService.editLine(new LineDTO(this.lineId, this.lineForm.controls.name.value,
         this.lineForm.controls.type.value, this.lineStationIds)).subscribe(
         result => {
            console.log(result)
            this.lines = result.data;
            window.alert('Line edited successful')
         },
         err => {
            console.log(err.error.message);
         } 
      );
   }

   deleteLine() {
      this.routeService.deleteLine(this.lineId).subscribe(
         result => {
            this.getLines();
            this.lineForm.controls.select.setValue('choose');
            this.lineForm.controls.name.setValue('');
            this.lineForm.controls.type.setValue('');
         },
         err => {
            console.log(err.error.message);
         }
      );
   }

   onSelect(event: any) {
      if (event.target.value != 'choose') {
         this.lineId = parseInt(event.target.value);
         for (let i = 0; i < this.lines.length; i++) {
            if (this.lines[i]['id'] == this.lineId) {
               this.lineForm.controls.name.setValue(this.lines[i]['name']);
               this.lineForm.controls.type.setValue(this.lines[i]['type']);
               this.lineStationCheck(this.lines[i]);
            }
         }
      }
   }

   lineStationCheck(line: LineDTO) {
      this.stationsCheck = [];
      for (let i = 0; i < this.stations.length; i++) {
         this.stationsCheck.push(false);
      }
      for (let i = 0; i < line['stationIds'].length; i++) {
         for (let j = 0; j < this.stations.length; j++) {
            if (line['stationIds'][i] == this.stations[j]['id']) {
               this.stationsCheck[j] = true;
               break;
            }
         }
      }
   }

   onCheckValueChange(id: number, i: number) {
      this.stationsCheck[i] = !this.stationsCheck[i];
   }
}
