import { Injectable } from '@angular/core';
import { User } from '../Models/User';
import { UserRegister } from '../Models/UserRegister';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Skill } from '../Models/Skill';
import { Project } from '../Models/Project';
import { AddProjectDto } from '../Models/DTO/AddProjectDto';
import { Observable } from 'rxjs';
import { Sprint } from '../Models/Sprint';
import { IssueLabel } from '../Models/IssueLabel';
import { PriorityLevel } from '../Models/PriorityLevel';
@Injectable({
  providedIn: 'root',
})
export class ProjectService {
  public projectData: Project | null = null;
  public userData: User | null = null;
  public selectedProject: Project | null = null;
  private apiUrl = 'https://localhost:7261';
  private projectApiUrl = `${this.apiUrl}/api/Project`;
  private userApiUrl = `${this.apiUrl}/User`;

  constructor(private http: HttpClient) {}

  setUserProject(project: Project) {
    localStorage.setItem('project', JSON.stringify(project));
  }

  getUserDataFromLocalStorage(): User | null {
    const user = localStorage.getItem('user');
    return user ? JSON.parse(user) : null;
  }

  getUserProjectsFromApi(email: string) {
    const url = `${this.projectApiUrl}/GetAllProjectsForMemeber?email=${encodeURIComponent(email)}`;
    return this.http.get<Project[]>(url).pipe(
      map((response: any) => {
        return response as Project[];
      })
    );
  }

  setSelectedProject(project: Project) {
    this.projectData = project;
    console.log(this.projectData);
    localStorage.setItem('selectedProject', JSON.stringify(project));
  }

  addProject(project: AddProjectDto, ownerId?: number): Observable<any> {
    const url = `${this.projectApiUrl}/AddProject?ownerId=${ownerId}`;
    return this.http.post(url, project).pipe(
      map((response) => {
        return response;
      })
    );
  }

  deleteProject(projectId: string): Observable<any> {
    const url = `${this.projectApiUrl}/DeleteProject?id=${projectId}`;
    return this.http.delete(url).pipe(
      map((response) => {
        return response;
      })
    );
  }

  leaveProject(projectId: string): Observable<any> {
    const url = `${this.projectApiUrl}/RemoveUserFromProject/${projectId}${this.userData?.email}`;
    return this.http.delete(url).pipe(
      map((response) => {
        return response;
      })
    );
  }

  getSelectedProject(): Project | null {
    if (this.selectedProject === null) {
      this.selectedProject = JSON.parse(
        localStorage.getItem('selectedProject') || 'null'
      );
    }
    return this.selectedProject;
  }

  joinProject(key: string): Observable<any> {
    var user = this.getUserDataFromLocalStorage();
    const url = `${this.userApiUrl}/JoinProject?email=${user?.email}&key=${key}`;
    return this.http.post(url, null).pipe(
      map((response) => {
        return response;
      })
    );
  }

  getAllMembers(): Observable<User[]> {
    var projectId = this.getSelectedProject()?.projectId;
    const url = `${this.projectApiUrl}/GetAllUsersForProject?id=${projectId}`;
    return this.http.get<User[]>(url).pipe(
      map((response) => {
        return response as User[];
      })
    );
  }

  getAllSprintsForProject(): Observable<Sprint[]> {
    const projectId = this.getSelectedProject()?.projectId;
    const url = `${this.projectApiUrl}/GetAllSprintsForProject?id=${projectId}`;
    return this.http.get<Sprint[]>(url).pipe(
      map((response) => {
        return response as Sprint[];
      })
    );
  }

  getAllAdministratorsForProject(): Observable<User[]> {
    const projectId = this.getSelectedProject()?.projectId;
    const url = `${this.projectApiUrl}/GetAllAdministratorsForProject?projectId=${projectId}`;
    return this.http.get<User[]>(url).pipe(
      map((response) => {
        return response as User[];
      })
    );
  }
  addAdminstratorToProject(email: string): Observable<any> {
    const projectId = this.getSelectedProject()?.projectId;
    const url = `${this.projectApiUrl}/AddAdministratorToProject?projectId=${projectId}&email=${email}`;
    return this.http.post(url, null).pipe(
      map((response) => {
        return response;
      })
    );
  }

  removeMemberFromProject(email:string): Observable<any> {
    const projectId = this.getSelectedProject()?.projectId;
    const url = `${this.projectApiUrl}/RemoveUserFromProject?projectId=${projectId}&email=${email}`;
    return this.http.put(url, null).pipe(
      map((response) => {
        return response;
      })
    );
  }
  getAllLabelsForProject(): Observable<IssueLabel[]> {
    const projectId = this.getSelectedProject()?.projectId;
    const url = `${this.projectApiUrl}/getLabelsForProject?projectId=${projectId}`;
    return this.http.get<IssueLabel[]>(url).pipe(
      map((response) => {
        return response as IssueLabel[];
      })
    );
  }
  removeIssueFromProject(issueId: string): Observable<any> {
    const projectId = this.getSelectedProject()?.projectId;
    const url = `${this.projectApiUrl}/removeIssueFromProject?projectId=${projectId}&issueId=${issueId}`;
    return this.http.delete(url).pipe(
      map((response) => {
        console.log("Repsonse" + response);
        return response;
      })
    );
  }
  addLabelToProject(label: IssueLabel): Observable<any> {
    const projectId = this.getSelectedProject()?.projectId;
    const url = `${this.projectApiUrl}/addLabelToProject?projectId=${projectId}`;
    return this.http.post(url, label).pipe(
      map((response) => {
        return response;
      })
    );
  }
  removeAdminstratorFromProject(email: string): Observable<any> {
    const projectId = this.getSelectedProject()?.projectId;
    const url = `${this.projectApiUrl}/RemoveAdministratorFromProject?projectId=${projectId}&email=${email}`;
    return this.http.delete(url).pipe(
      map((response) => {
        return response;
      })
    );
  }
  getPriorities():string[]
  {
    var priorities = [
      {
        value: PriorityLevel.Low,
        name: 'Low',
        color: 'green',
        icon: 'arrow_downward',
        id:0
      },
      {
        value: PriorityLevel.Medium,
        name: 'Medium',
        color: '#DAA520',
        icon: 'arrow_right_alt',
        id:1
      },
      {
        value: PriorityLevel.High,
        name: 'High',
        color: 'orange',
        icon: 'arrow_upward',
        id:2
      },
      {
        value: PriorityLevel.Critical,
        name: 'Critical',
        color: 'red',
        icon: 'error',
        id:3
      },
    ];
    return priorities.map(x=>x.id.toString())
  }
}