import { Injectable } from '@angular/core';
import { User } from '../Models/User'; // asigurați-vă că calea este corectă
import { UserRegister } from '../Models/UserRegister';
import { HttpClient } from '@angular/common/http';
import{map} from 'rxjs/operators';
import { Skill } from '../Models/Skill';
import { Issue } from '../Models/Issue';
import { Project } from '../Models/Project';
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
  getUserById(id: string) {
    return this.http.get<User>(`${this.apiUrl}/User/getUserById?id=${id}`).pipe(
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
  getAllUserIssues(Id: string) {
    return this.http.get<Issue[]>(`${this.apiUrl}/User/getUserIssues?Id=${Id}`).pipe(
      map(response => {
        return response as Issue[];
      })
    );
  }
  getAllUserProject(email: string) {
    return this.http.get<Project[]>(`${this.apiUrl}/User/getUserProjects?email=${email}`).pipe(
      map(response => {
        return response as Project[];
      })
    );
  }
  getAllIssuesForProject(id: string) {
    return this.http.get<Issue[]>(`${this.apiUrl}/Issue/getAllIssuesForProject?id=${id}`).pipe(
      map(response => {
        return response as Issue[];
      })
    );
  }

  updateProfilePhoto(id: string, photo: string) {
    // Construieste URL-ul cu parametrii de interogare
    const url = `https://localhost:7261/User/updateUserPhoto?id=${id}&photo=${photo}`;

    // Trimite cererea POST la URL
    return this.http.post(url, {});
}
  logOut()
  {
    if (localStorage.getItem('user')) {
      localStorage.removeItem('user');
      localStorage.removeItem('skills');
      this.userData = null;
    }
  }
}
