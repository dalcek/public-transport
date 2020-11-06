import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminLinesComponent } from './components/admin-lines/admin-lines.component';
import { AdminMapComponent } from './components/admin-map/admin-map.component';
import { AdminPricelistComponent } from './components/admin-pricelist/admin-pricelist.component';
import { AdminStationsComponent } from './components/admin-stations/admin-stations.component';
import { AdminTimetableComponent } from './components/admin-timetable/admin-timetable.component';
import { BuyATicketComponent } from './components/buy-a-ticket/buy-a-ticket.component';
import { ControllerTicketComponent } from './components/controller-ticket/controller-ticket.component';
import { ControllerUserComponent } from './components/controller-user/controller-user.component';
import { LoginComponent } from './components/login/login.component';
import { MapComponent } from './components/map/map.component';
import { PricelistComponent } from './components/pricelist/pricelist.component';
import { ProfileComponent } from './components/profile/profile.component';
import { RegisterComponent } from './components/register/register.component';
import { TimetableComponent } from './components/timetable/timetable.component';
import { AdminGuard } from './guards/admin.guard';
import { ControllerGuard } from './guards/controller.guard';
import { LoggedInGuard } from './guards/logged-in.guard';
import { VisitorGuard } from './guards/visitor.guard';


const routes: Routes = [{
  path: '',
  redirectTo: 'login',
  pathMatch: 'full'
},
{
  path: 'login',
  component: LoginComponent
},
{
  path: 'register',
  component: RegisterComponent
},
{
  path: 'profile',
  component: ProfileComponent,
  canActivate: [LoggedInGuard]
},
{
  path: 'pricelist',
  component: PricelistComponent
},
{
  path: 'buy-a-ticket',
  component: BuyATicketComponent,
  canActivate: [VisitorGuard]
},
{
   path: 'timetable',
   component: TimetableComponent
},
{
   path: 'map',
   component: MapComponent
},
{
  path: 'admin-pricelist',
  component: AdminPricelistComponent,
  canActivate: [AdminGuard]
},
{
   path: 'admin-timetable',
   component: AdminTimetableComponent,
   canActivate: [AdminGuard]
},
{
   path: 'admin-stations',
   component: AdminStationsComponent,
   canActivate: [AdminGuard]
},
{
   path: 'admin-lines',
   component: AdminLinesComponent,
   canActivate: [AdminGuard]
},
{
   path: 'admin-map',
   component: AdminMapComponent,
   canActivate: [AdminGuard]
},
{
   path: 'controller-ticket',
   component: ControllerTicketComponent,
   canActivate: [ControllerGuard]
},
{
   path: 'controller-user',
   component: ControllerUserComponent,
   canActivate: [ControllerGuard]
}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
