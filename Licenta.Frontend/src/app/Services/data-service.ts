import { Injectable } from '@angular/core';
import { User } from '../Models/User'; // asigurați-vă că calea este corectă
import { UserRegister } from '../Models/UserRegister';
import { HttpClient } from '@angular/common/http';
import{map} from 'rxjs/operators';
import { Skill } from '../Models/Skill';
@Injectable({
  providedIn: 'root'
})
export class DataService {
  public userData: User | null = null; // inițializat ca null
  public registerData: UserRegister | null = null; // inițializat ca null
  private apiUrl = 'https://localhost:7261';

  constructor(private http: HttpClient) { }

  setUserData(user: User) {
    localStorage.setItem('user', JSON.stringify(user));
  }
  setUserSkills(skills: Skill[]) {
    localStorage.setItem('skills', JSON.stringify(skills));
  }
  
  getUserData(): User | null {
    const user = localStorage.getItem('user');
    //add users skills
    return user ? JSON.parse(user) : null;
  }
  addUser(user: UserRegister) {
    return this.http.post(this.apiUrl + '/register', user);
  }
  loginUser(user: UserRegister) {
    console.log(this.http.post(this.apiUrl + '/login', user));
    return this.http.post(this.apiUrl + '/login', user);
  }
  getUser(email: string) {
    return this.http.get<User>(this.apiUrl + '/User/getUser/' + email).pipe(
      map(response => {
        return response as User;
      })

    );
  }
  getUsersSkills(email: string) {
    return this.http.get<Skill[]>(this.apiUrl + '/User/getUserSkills/' + email).pipe(
      map(response => {
        return response as Skill[];
      })
    );
  }
}
