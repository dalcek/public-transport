import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { TokenInterceptor } from './interceptors/token.interceptor';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { LoginComponent } from './components/login/login.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RegisterComponent } from './components/register/register.component';
import { ProfileComponent } from './components/profile/profile.component';
import { PricelistComponent } from './components/pricelist/pricelist.component';
import { BuyATicketComponent } from './components/buy-a-ticket/buy-a-ticket.component';
import { NgxPayPalModule } from 'ngx-paypal';
import { AdminPricelistComponent } from './components/admin-pricelist/admin-pricelist.component';
import { TimetableComponent } from './components/timetable/timetable.component';
import { AdminTimetableComponent } from './components/admin-timetable/admin-timetable.component';
import { AdminStationsComponent } from './components/admin-stations/admin-stations.component';
import { AdminLinesComponent } from './components/admin-lines/admin-lines.component';
import { ControllerTicketComponent } from './components/controller-ticket/controller-ticket.component';
import { ControllerUserComponent } from './components/controller-user/controller-user.component';
import { AdminMapComponent } from './components/admin-map/admin-map.component';
import { AgmCoreModule } from '@agm/core';
import { MapComponent } from './components/map/map.component';

import { SocketIoModule, SocketIoConfig } from 'ngx-socket-io';

const config: SocketIoConfig = { url: 'http://localhost:3000', options: {} };

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    LoginComponent,
    RegisterComponent,
    ProfileComponent,
    PricelistComponent,
    BuyATicketComponent,
    AdminPricelistComponent,
    TimetableComponent,
    AdminTimetableComponent,
    AdminStationsComponent,
    AdminLinesComponent,
    ControllerTicketComponent,
    ControllerUserComponent,
    AdminMapComponent,
    MapComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    NgxPayPalModule,
    AgmCoreModule.forRoot({apiKey: 'AIzaSyDnihJyw_34z5S1KZXp90pfTGAqhFszNJk'}),
    SocketIoModule.forRoot(config)
  ],
  providers: [{provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true}],
  bootstrap: [AppComponent]
})
export class AppModule { }
