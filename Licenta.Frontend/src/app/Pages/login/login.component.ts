// login.component.ts
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { DataService } from '../../Services/data-service';
import { HttpClient } from '@angular/common/http';
import { User } from '../../Models/User';
import {map} from 'rxjs/operators';
import { UserRegister } from '../../Models/UserRegister';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent {
  loginForm: FormGroup;
  User:UserRegister;
  userData: User | null = null; // inițializat ca null

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private http: HttpClient,
    private userService: DataService,
  ) {
    this.loginForm = this.formBuilder.group({
      email: ['', Validators.required],
      password: ['', Validators.required],
    });
    this.User = { email: '', password: ''}; // Inițializare User
    this.userData={email:'',firstName:'',lastName:'', role:0,id:0,profilePicture:'',fullName:'', skills:[]}; // Inițializare User
  }
  loginUser(email: string, password: string) {
  return this.http.post('https://localhost:7261/login', { email, password });
  }
  getUser(email:string)
  {
    return this.userService.getUser(email).pipe(
      map(response => {
        return response as User;
      })
    );
  }
  onLogin(event: Event) {
    event.preventDefault();
    if (this.loginForm.valid) {
      const email = this.loginForm.value.email;
      const password = this.loginForm.value.password;
      if (email && password) {
        this.loginUser(email, password).subscribe((response) => {
          this.getUser(email).subscribe((response) => {
            this.userData = response;
            console.log(this.userData);
            this.userService.setUserData(this.userData);
            this.router.navigate(['/home']);
          });
          if (response) {
            console.log('Login successful');
          }
            else{
           {
            console.log('Login failed'); // Add this line
            window.alert(
              'Login failed. Please check your username and password.'
            );
          }
        }
        });
      }
      // console.log(this.loginForm.value);
    }
}
goToRegister() {
  this.router.navigate(['/register']);
}
}
