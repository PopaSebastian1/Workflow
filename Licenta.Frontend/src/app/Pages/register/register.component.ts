import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent {
  registerForm: FormGroup;
  baseUrl = 'https://localhost:7261';
  images = [
    { name: 'Image 1', url: 'https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcTjo-4GdQ00VYxRdpbKtXs0DxjjYi_4iY8w82SxAeBFOMHnujQM' },
    { name: 'Image 2', url: 'https://static.vecteezy.com/system/resources/previews/005/036/044/non_2x/trendy-teacher-concepts-vector.jpg' },
    { name: 'Image 3', url: 'https://cdn3.iconfinder.com/data/icons/business-avatar-1/512/11_avatar-512.png' },
    { name: 'Image 4', url: 'https://cdn.icon-icons.com/icons2/2643/PNG/512/male_boy_person_people_avatar_icon_159358.png' },
    { name: 'Image 5', url: 'https://banner2.cleanpng.com/20210721/osx/transparent-jedi-avatar-60f79d68da49c0.1144258416268404248941.jpg' },
    { name: 'Image 6', url: 'https://w7.pngwing.com/pngs/129/292/png-transparent-female-avatar-girl-face-woman-user-flat-classy-users-icon.png' },
  ];
  profilePicture!:string;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private http: HttpClient
  ) {
    this.registerForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, this.passwordValidator]],
      firstName: [''],
      lastName: [''],
    });
    this.profilePicture = this.images[0].url;
  }

  selectProfilePhoto(photoUrl: string) {
    this.profilePicture = photoUrl;
  }

  registerUser(email: string, password: string) {
    const url = `${this.baseUrl}/register`;
    const body = { email, password };
    const options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'accept': '*/*',
      }),
    };

    return this.http.post(url, body, options).pipe(
      catchError((error) => {
        console.error('Registration error:', error);
        return throwError(error);
      })
    );
  }

  updateUser(email: string, firstName: string, lastName: string, profilePicture: string) {
    const url = `${this.baseUrl}/User/updateUser?email=${email}&firstName=${firstName}&lastName=${lastName}&profilePicture=${profilePicture}`;
    const options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'accept': '*/*',
      }),
      observe: 'response' as 'response',
    };

    return this.http.post(url, {}, options).pipe(
      catchError((error) => {
        console.error('Update user error:', error);
        return throwError(error);
      })
    );
  }

  onRegister(event: Event) {
    event.preventDefault();
   
    if (!this.registerForm.valid) {
      window.alert('Form is not valid. Please check your details and try again.');
      return;
    }
  
    const { email, password, firstName, lastName } = this.registerForm.value;
  
      this.registerUser(email, password).subscribe({
      next: (registrationResponse) => {
        console.log('Registration successful:', registrationResponse);
        this.updateUser(email, firstName, lastName, this.profilePicture).subscribe({
          next: (updateResponse) => {
            if (updateResponse.status === 200) {
              console.log('Update successful:', updateResponse);
              this.router.navigate(['/login']);
            } else {
              console.error('Update failed:', updateResponse);
              window.alert('Update failed. Please check your details and try again.');
            }
          },
          error: (updateError) => {
            console.error('Update error:', updateError);
            window.alert('Update failed. Please check your details and try again.');
          }
        });
      },
      error: (registrationError) => {
        console.error('Registration failed:', registrationError);
        window.alert('Registration failed. Please check your details and try again.');
      }
    });
  }
  passwordValidator(control: AbstractControl): { [key: string]: boolean } | null {
    const value = control.value;
    if (!value) {
      return null;
    }
    const hasUpperCase = /[A-Z]/.test(value);
    const hasLowerCase = /[a-z]/.test(value);
    const hasNumeric = /[0-9]/.test(value);
    const hasSpecialCharacter = /[!@#$%^&*(),.?":{}|<>]/.test(value);
    const isValidLength = value.length >= 8;

    const passwordValid = hasUpperCase && hasLowerCase && hasNumeric && hasSpecialCharacter && isValidLength;
    if (!passwordValid) {
      return { passwordStrength: true };
    }
    return null;
  }
  get hasLengthRequirement() {
    const password = this.registerForm.get('password')!.value;
    return password && password.length >= 8;
  }
  
  get hasUpperCaseRequirement() {
    const password = this.registerForm.get('password')!.value;
    return password && /[A-Z]/.test(password);
  }
  
  get hasLowerCaseRequirement() {
    const password = this.registerForm.get('password')!.value;
    return password && /[a-z]/.test(password);
  }
  
  get hasNumericRequirement() {
    const password = this.registerForm.get('password')!.value;
    return password && /[0-9]/.test(password);
  }
  
  get hasSpecialCharacterRequirement() {
    const password = this.registerForm.get('password')!.value;
    return password && /[!@#$%^&*(),.?":{}|<>]/.test(password);
  }
}
