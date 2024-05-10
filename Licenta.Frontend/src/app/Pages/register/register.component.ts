import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
registerForm: FormGroup;

constructor(private formBuilder: FormBuilder, private router: Router, private http: HttpClient) {
  this.registerForm = this.formBuilder.group({
    email: ['', Validators.required],
    password: ['', Validators.required],
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
  });
  
}
registerUser(email: string, password: string) {
  return this.http.post(
    'https://localhost:7261/register',
    { email, password }, { observe: 'response' }
  );
}

updateUser(email:string, firstName:string, lastName:string) {
  return this.http.post(
    'https://localhost:7261/User/updateUser?email=' + email + '&firstName=' + firstName + '&lastName=' + lastName,
    { email, firstName, lastName }, { observe: 'response' }
  );
}
onRegister(event: Event) {
  event.preventDefault();
  if (this.registerForm.valid) {
    const email = this.registerForm.value.email;
    const password = this.registerForm.value.password;
    const firstName = this.registerForm.value.firstName;
    const lastName= this.registerForm.value.lastName;
    if (email && password) {
      this.registerUser(email, password).subscribe((response) => {
        if (response.status === 200) {
          console.log(response.status);
          // this.router.navigate(['/login']);
          this.updateUser(email, firstName, lastName).subscribe((response) => {
            if (response.status === 200) {
              console.log(response.status);
              this.router.navigate(['/login']);
            } else {
              console.log('Update failed'); // Add this line
              window.alert(
                'Update failed. Please check your username and password.'
              );
            }
          });
        } else {
          console.log('Registration failed'); // Add this line
          window.alert(
            'Registration failed. Please check your username and password.'
          );
        }
      });
    }
    console.log(this.registerForm.value);
  }
}
}

