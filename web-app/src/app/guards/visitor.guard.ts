import { Injectable } from '@angular/core';
import { CanActivate, CanActivateChild, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class VisitorGuard implements CanActivate, CanActivateChild {
   constructor(private router: Router) { }

   canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {    
     if (localStorage.role != 'Controller' && localStorage.role != 'Admin') {
       return true;
     }
     else {
       console.error('Access denied. Log out or log in as an AppUser.');
       this.router.navigate(['profile']);
       return false;
     }
   }
 
   canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
     return this.canActivate(route, state);
   }
}