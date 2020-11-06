import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Subscription } from 'rxjs';
import { CoordinateDTO, GeoLocation, LineDTO, MarkerInfo, Station } from 'src/app/models/models';
import { LocationService } from 'src/app/services/location/location.service';
import { RouteService } from 'src/app/services/route/route.service';

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css']
})
export class MapComponent implements OnInit {
   
   lineForm = this.formBuilder.group({
      selectLine: ['choose'],
   });
   
   private _busSub: Subscription;

   stations: Station[];
   stationsToShow: Station[];
   lines: LineDTO[];
   lineId: number;
   selectedLine: LineDTO;
   lineCoords: CoordinateDTO[] = [];
   busLocation: GeoLocation = new GeoLocation(-1, -1);
   showBus: boolean = false;

   markerIconUrl: string;
   busIconUrl: string;
   markerInfo: MarkerInfo;

   constructor(private routeService: RouteService, private locationService: LocationService, private formBuilder: FormBuilder) { }

   ngOnInit(): void {
      this.markerInfo = new MarkerInfo(new GeoLocation(45.250083, 19.830849),
      "../../../assets/markericon.png",
      "Jugodrvo", "", "http://ftn.uns.ac.rs/691618389/fakultet-tehnickih-nauka");
      this.busIconUrl = "../../../assets/busicon.png";
      this.markerIconUrl = "../../../assets/markericon.png";
      this.subscribeToBusLocation()
      this.locationService.message.subscribe(result => console.log(result));
      //this._busSub = this.locationService.busLocation.subscribe(result => console.log(result))
      this.getStations();
      this.getLines();
   }

   ngOnDestroy() {
      this._busSub.unsubscribe();
      this.locationService.stopBusLocation()
   }


   // send() {
   //    this.locationService.sendMessage('cao')
   // }

   location() {
      this.locationService.requestBusLocation(1);
   }

   subscribeToBusLocation() {
      this._busSub = this.locationService.busLocation.subscribe(
         result => {
            console.log(result);
            this.busLocation.latitude = parseFloat(result.split('|')[0])
            this.busLocation.longitude = parseFloat(result.split('|')[1]) 
         }, err => {
            console.log(err);
         }
      );
   }

   onSelect(event: any) {
      this.lineCoords = [];
      this.stationsToShow = [];
      this.busLocation = new GeoLocation(-1, -1)
      this.locationService.stopBusLocation()
      this.showBus = false
      if (event.target.value != 'choose') {
         this.lineId = parseInt(event.target.value);
         this.showBus = true;
         this.locationService.requestBusLocation(this.lineId);
         this.getLineRoute();
         console.log(this.lineCoords);
         for (let i = 0; i < this.lines.length; i++) {
            if (this.lines[i]['id'] == this.lineId) {
               this.selectedLine = this.lines[i];
               break;
            }
         }
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
}
