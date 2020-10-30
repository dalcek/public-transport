import { NgLocalization } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { addListener } from 'process';
import { AddLineDTO, AddStationDTO, CoordinateDTO, GeoLocation, LineDTO, MarkerInfo, Station } from 'src/app/models/models';
import { RouteService } from 'src/app/services/route/route.service';


@Component({
  selector: 'app-admin-map',
  templateUrl: './admin-map.component.html',
  styleUrls: ['./admin-map.component.css']
})
export class AdminMapComponent implements OnInit {

   stationForm = this.formBuilder.group({
      name: ['', Validators.required],
      address: ['', Validators.required],
      xCoordinate: ['', Validators.required],
      yCoordinate: ['', Validators.required],
   });

   lineForm = this.formBuilder.group({
      selectLine: ['choose'],
      name: ['', Validators.required],
      type: ['City', Validators.required]
   });
   
   stations: Station[];
   stationsToShow: Station[];
   lines: LineDTO[];
   lineId: number;
   selectedLine: LineDTO;

   lineCoords: CoordinateDTO[] = [];
   newLineCoords: CoordinateDTO[] = [];

   currentTab: string = 'stations';

   markerIcon: string = "../../../../markericon.png";
   markerInfo: MarkerInfo = new MarkerInfo(new GeoLocation(45.251302, 19.810568),
   "../../../../reg_img.png",
   "Jugodrvo", "", "http://ftn.uns.ac.rs/691618389/fakultet-tehnickih-nauka");

   constructor(private formBuilder: FormBuilder, private routeService: RouteService) { }

   ngOnInit(): void {
      this.getStations();
      this.getLines();
   }


   placeMarker($event) {
      console.log($event);
      
      if (this.currentTab == 'stations') {
         this.stationForm.controls.xCoordinate.setValue($event.coords.lat);
         this.stationForm.controls.yCoordinate.setValue($event.coords.lng);
      }
      else if (this.lineForm.controls.selectLine.value == 'add'){
         console.log('linecoords');
         console.log(this.newLineCoords);
         this.newLineCoords.push(new CoordinateDTO(parseFloat($event.coords.lat), parseFloat($event.coords.lng)));
      }
   }

   onStationTab() {
      this.stationForm.controls.name.setValue('');
      this.stationForm.controls.address.setValue('');
      this.stationForm.controls.xCoordinate.setValue('');
      this.stationForm.controls.yCoordinate.setValue('');
      this.currentTab = 'stations';
      this.stationsToShow = this.stations;
      this.lineCoords = [];
      this.stationsToShow = [];
      this.newLineCoords = [];
   }
   onLineTab() {
      this.newLineCoords = [];
      this.lineCoords = [];
      this.stationsToShow = [];
      this.lineForm.controls.selectLine.setValue('choose');
      this.lineForm.controls.name.setValue('');
      this.lineForm.controls.type.setValue('City');
      this.currentTab = 'lines';
   }

   onSelect(event: any) {
      this.newLineCoords = [];
      this.lineCoords = [];
      this.stationsToShow = [];
      this.lineForm.controls.name.setValue('');
      this.lineForm.controls.type.setValue('City');
      if (event.target.value != 'choose' && event.target.value != 'add') {
         this.lineId = parseInt(event.target.value);
         this.getLineRoute();
         console.log(this.lineCoords);
         for (let i = 0; i < this.lines.length; i++) {
            if (this.lines[i]['id'] == this.lineId) {
               this.selectedLine = this.lines[i];
               break;
            }
         }
         this.lineForm.controls.name.setValue(this.selectedLine['name']);
         this.lineForm.controls.type.setValue(this.selectedLine['type']);
         for (let i = 0; i < this.selectedLine['stationIds'].length; i++) {
            for (let j = 0; j < this.stations.length; j++) {
               if (this.selectedLine['stationIds'][i] == this.stations[j]['id']) {
                  this.stationsToShow.push(this.stations[j]);
                  break;
               }
            }
         }
      }
   }

   getStations() {
      this.routeService.getStations().subscribe(
         result => {
            console.log(result);
            this.stations = result.data;
            this.stationsToShow = result.data;
         },
         err => {
            console.log(err.error.message);
         }
      );
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

   getLineRoute() {
      this.routeService.getLineRoute(this.lineId).subscribe(
         result => {
            console.log(result);
            this.lineCoords = result.data;
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

   addLine() {
      this.routeService.addLine(new AddLineDTO(this.lineForm.controls.name.value, 
         this.lineForm.controls.type.value, this.newLineCoords)).subscribe(
         result => {
            window.alert(`Line ${result.data} successfully added.`);
            this.getLines();
            this.newLineCoords = [];
            this.lineForm.controls.name.setValue('');
            this.lineForm.controls.type.setValue('');
         },
         err => {
            console.log(err.error.message);
         }
      );
   }
}
