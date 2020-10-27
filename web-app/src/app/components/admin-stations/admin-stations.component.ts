import { Route } from '@angular/compiler/src/core';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { timeStamp } from 'console';
import { resourceUsage } from 'process';
import { AddStationDTO, Station } from 'src/app/models/models';
import { RouteService } from 'src/app/services/route/route.service';

@Component({
  selector: 'app-admin-stations',
  templateUrl: './admin-stations.component.html',
  styleUrls: ['./admin-stations.component.css']
})
export class AdminStationsComponent implements OnInit {

   stationForm = this.formBuilder.group({
      select: ['choose'],
      name: ['', Validators.required],
      address: ['', Validators.required],
      xCoordinate: ['', Validators.required],
      yCoordinate: ['', Validators.required],
   });

   stations: Station[];
   stationId: number;

   constructor(private formBuilder: FormBuilder, private routeService: RouteService) { }

   ngOnInit(): void {
      this.getStations();
   }

   onSelect(event: any) {
      if (event.target.value != 'choose' && event.target.value != 'add') {
         this.stationId = parseInt(event.target.value);
         for (let i = 0; i < this.stations.length; i++) {
            if (this.stations[i]['id'] == this.stationId) {
               this.stationForm.controls.name.setValue(this.stations[i]['name']);
               this.stationForm.controls.address.setValue(this.stations[i]['address']);
               this.stationForm.controls.xCoordinate.setValue(this.stations[i]['xCoordinate']);
               this.stationForm.controls.yCoordinate.setValue(this.stations[i]['yCoordinate']);
               break;
            }
         }
      }
   }

   getStations() {
      this.routeService.getStations().subscribe(
         result => {
            console.log(result);
            this.stations = result.data;
         },
         err => {
            console.log(err.error.message);
         }
      );
   }

   addStation() {
      this.routeService.addStation(new AddStationDTO(this.stationForm.controls.name.value, this.stationForm.controls.address.value,
            this.stationForm.controls.xCoordinate.value, this.stationForm.controls.yCoordinate.value)).subscribe(
         result => {
            this.stations = result.data;
            this.stationForm.controls.select.setValue('choose');
            this.stationForm.controls.name.setValue('');
            this.stationForm.controls.address.setValue('');
            this.stationForm.controls.xCoordinate.setValue('');
            this.stationForm.controls.yCoordinate.setValue('');
         },
         err => {
            console.log(err.error.message);
         }
      );
   }

   editStation() {
      this.routeService.editStation(new Station(this.stationId, this.stationForm.controls.name.value, this.stationForm.controls.address.value,
            this.stationForm.controls.xCoordinate.value, this.stationForm.controls.yCoordinate.value)).subscribe(
         result => {
            this.stations = result.data;
            this.stationForm.controls.select.setValue('choose');
            this.stationForm.controls.name.setValue('');
            this.stationForm.controls.address.setValue('');
            this.stationForm.controls.xCoordinate.setValue('');
            this.stationForm.controls.yCoordinate.setValue('');
         },
         err => {
            console.log(err.error.message);
         }
      );
   }

   deleteStation() {
      this.routeService.deleteStation(this.stationId).subscribe(
         result => {
            this.stations = result.data;
            this.stationForm.controls.select.setValue('choose');
            this.stationForm.controls.name.setValue('');
            this.stationForm.controls.address.setValue('');
            this.stationForm.controls.xCoordinate.setValue('');
            this.stationForm.controls.yCoordinate.setValue('');
         },
         err => {
            console.log(err.error.message);
         }
      );
   }
}
