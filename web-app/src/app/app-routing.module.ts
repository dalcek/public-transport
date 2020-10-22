import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminPricelistComponent } from './components/admin-pricelist/admin-pricelist.component';
import { BuyATicketComponent } from './components/buy-a-ticket/buy-a-ticket.component';
import { LoginComponent } from './components/login/login.component';
import { PricelistComponent } from './components/pricelist/pricelist.component';
import { ProfileComponent } from './components/profile/profile.component';
import { RegisterComponent } from './components/register/register.component';


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
  component: ProfileComponent
},
{
  path: 'pricelist',
  component: PricelistComponent
},
{
  path: 'buy-a-ticket',
  component: BuyATicketComponent
},
{
  path: 'admin-pricelist',
  component: AdminPricelistComponent
}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
