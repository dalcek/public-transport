import { Injectable } from '@angular/core';
import { Socket } from 'ngx-socket-io';
@Injectable({
  providedIn: 'root'
})
export class LocationService {

  constructor(private socket: Socket) { }

   message =  this.socket.fromEvent<string>('message');
   //message: string = "";
   busLocation = this.socket.fromEvent<string>('location');


   sendMessage(msg: string) {
      this.socket.emit('my message', msg);
   }

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
