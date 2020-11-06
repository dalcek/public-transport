import { Injectable } from '@angular/core';
import { Socket } from 'ngx-socket-io';
@Injectable({
  providedIn: 'root'
})
export class LocationService {

  constructor(private socket: Socket) { }

   message = this.socket.fromEvent<string>('my message');
   busLocation = this.socket.fromEvent<string>('location');

   sendMessage(msg: string) {
      this.socket.emit('my message', msg);
   }

   requestBusLocation(lineId: number) {
      this.socket.emit('location', lineId);
   }

   stopBusLocation() {
      this.socket.emit('disconnect')
   }

   // receiveMessage() {
   //    this.socket.on('my message', (message) => {
   //       console.log(message)
   //    })
   // }

}
