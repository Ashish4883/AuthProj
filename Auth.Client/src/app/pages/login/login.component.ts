import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  toggleForm: boolean = false;
  registerObj: any = {
    userId: 0,
    emailId: '',
    password: '',
    createdDate: new Date(),
    fullName: '',
    mobileNo: '',
  };

  loginObj: any = {
    emailId: '',
    password: '',
  };

  http = inject(HttpClient);
  router = inject(Router);

  toggle() {
    this.toggleForm = !this.toggleForm;
  }

  onRegister() {
    this.http
      .post('https://localhost:7268/api/User/CreateNewUser', this.registerObj)
      .subscribe(
        (res: any) => {
          alert('Registration success, Do Login!!');
          this.toggle();
        },
        (error) => {
          if (error.status == 400) {
            alert('Invalid body');
          } else if (error.status == 509) {
            alert(error.error);
          }
        }
      );
  }

  onLogin() {
    this.http
      .post('https://localhost:7268/api/User/Login', this.loginObj)
      .subscribe(
        (res: any) => {
          alert('Login success');
          localStorage.setItem('token', res.token);
          localStorage.setItem('authUser', JSON.stringify(res.user));
          this.router.navigateByUrl('user-list');
        },
        (error) => {
          if (error.status == 401) {
            alert(error.error);
          }
        }
      );
  }
}
