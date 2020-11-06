import { Injectable } from '@angular/core';
import { CanActivate, CanActivateChild, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoggedInGuard implements CanActivate, CanActivateChild {
   constructor(private router: Router) { }

   canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {    
      if (localStorage.role == 'AppUser' || localStorage.role == 'Admin' || localStorage.role == 'Controller') {
         return true;
      }
      else {
         window.alert('Access denied for not registered users.');
         console.error('Access denied for not registered users.');
         this.router.navigate(['register']);
         return false;
      }
   }
 
   canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
     return this.canActivate(route, state);
   }
}
