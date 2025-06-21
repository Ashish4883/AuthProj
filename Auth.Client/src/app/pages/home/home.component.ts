import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { jwtDecode, JwtPayload } from 'jwt-decode';

@Component({
  selector: 'app-home',
  imports: [RouterOutlet],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent implements OnInit {
  router = inject(Router);
  role: string = '';

  ngOnInit(): void {
    this.getUserRole();
  }

  onLogout() {
    localStorage.clear();
    this.router.navigateByUrl('login');
  }

  getUserRole() {
    const token = localStorage.getItem('token');
    if (token) {
      const decoded: any = jwtDecode<JwtPayload>(token);
      this.role =
        decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      return this.role;
    }
    return null;
  }
}
