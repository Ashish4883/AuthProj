import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-user-list',
  imports: [],
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.css',
})
export class UserListComponent implements OnInit {
  userList: any[] = [];
  role: string = '';
  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.role = this.getUserRole();
    this.getUsers(this.role);
  }

  getUsers(role: string) {
    if (role == 'admin') {
      this.http
        .get('https://localhost:7268/api/User/getUsersForAdmin')
        .subscribe((res: any) => {
          this.userList = res;
        });
    } else if (role == 'user') {
      this.http
        .get('https://localhost:7268/api/User/getUsersForUser')
        .subscribe((res: any) => {
          this.userList = res;
        });
    }
  }

  getUserRole() {
    const token = localStorage.getItem('token');
    if (token) {
      const decoded: any = jwtDecode(token);
      return decoded[
        'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
      ];
    }
    return null;
  }
}
