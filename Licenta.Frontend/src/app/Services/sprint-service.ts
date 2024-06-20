import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Issue } from '../Models/Issue';
import { Sprint } from '../Models/Sprint';

@Injectable({
  providedIn: 'root',
})
export class SprintService {
  private baseUrl = 'https://localhost:7261/api';

  constructor(private http: HttpClient) {
  
  }

  getSprintsByProjectId(projectId: string): Observable<Sprint[]> {
    const url = `${this.baseUrl}/Sprint/GetSprintsByProjectId?projectId=${projectId}`;
    return this.http.get(url).pipe(
      map((response: any) => {
        return response as Sprint[];
      })
    );
  }
  getAllIssuesBySprint(sprintId: string): Observable<Issue[]> {
    const url = `${this.baseUrl}/Sprint/GetAllIssuesForSprint?id=${sprintId}`;
    return this.http.get(url).pipe(
      map((response: any) => {
        return response as Issue[];
      })
    );
  }
  updateSprintStatus(sprintId:string, status:number) {
    const url = `${this.baseUrl}/Sprint/UpdateSprintStatus?Id=${sprintId}&status=${status}`;
    return this.http.patch(url, {}).pipe(
      map((response) => {
        return response;
      })
    );
  }
  addSprint(sprint: Sprint): Observable<Sprint> {
    const url = `${this.baseUrl}/Sprint/AddSprint`;
    return this.http.post(url, sprint).pipe(
      map((response) => {
        return response as Sprint;
      })
    );
  }
}
