import { Injectable } from '@angular/core';
import { CanActivate, CanActivateChild, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate, CanActivateChild {
  
   constructor(private router: Router) { }

   canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {    
      if (localStorage.role === 'Admin') {
         return true;
      }
      else {
         console.error('Access denied for non admin users.');
         window.alert('Access denied for non admin users.');
         if(localStorage.role == null){
            this.router.navigate(['login']);
            return false;
         }
      }
   }
 
   canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
     return this.canActivate(route, state);
   }
}
