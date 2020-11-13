import { Injectable } from '@angular/core';
import { Socket } from 'ngx-socket-io';
@Injectable({
  providedIn: 'root'
})
export class LocationService {

  constructor(private socket: Socket) { }

   busLocation = this.socket.fromEvent<string>('location');

   requestBusLocation(lineId: string) {
      console.log('locs')
      this.socket.emit('location', lineId);
   }

   leaveRoom(room: string) {
      this.socket.emit('leave', room);
   }

   stopBusLocation() {
      this.socket.emit('disconnect')
   }
}
