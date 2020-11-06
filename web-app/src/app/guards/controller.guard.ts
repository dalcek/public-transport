import { Injectable } from '@angular/core';
import { CanActivate, CanActivateChild, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ControllerGuard implements CanActivate, CanActivateChild {

   constructor(private router: Router) { }

   canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {    
      if (localStorage.role === 'Controller') {
         return true;
      }
      else {
         console.error('Access denied for non controller users.');
         window.alert('Access denied for non controller users.');
         if(localStorage.role === null){
            this.router.navigate(['login']);
            return false;
         }
      }
   }

   canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
      return this.canActivate(route, state);
   }
}
