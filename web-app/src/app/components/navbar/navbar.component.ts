import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  loggedIn: string = localStorage['role'];

  constructor() { }

  ngOnInit(): void {
  }

  logout() {
    this.loggedIn = null;
    localStorage.clear();
    window.location.href = "/login";
  }
}
