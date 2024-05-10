import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Issue } from '../Models/Issue';
import { AddIssueDTO } from '../Models/DTO/AddIssueDTO';

@Injectable({
  providedIn: 'root',
})
export class IssueService {
  private baseUrl = 'https://localhost:7261';

  constructor(private http: HttpClient) {}

  getIssuesByProjectId(projectId: string): Observable<Issue[]> {
    const url = `${this.baseUrl}/Issue/GetIssueByProjectId/${projectId}`;
    return this.http.get(url).pipe(
      map((response: any) => {
        return response as Issue[];
      })
    );
  }
  addIssue(issue: AddIssueDTO): Observable<AddIssueDTO> {
    const url = 'https://localhost:7261/Issue/AddIssue';
    return this.http.post(url, issue).pipe(
      map((response) => {
        return response as AddIssueDTO;
      })
    );
  }
  getIssue(id: string): Observable<Issue> {
    const url = `${this.baseUrl}/Issue/GetIssue/${id}`;
    return this.http.get(url).pipe(
      map((response) => {
        return response as Issue;
      })
    );
  }
  getAllChildIssues(id: string): Observable<Issue[]> {
    const url = `${this.baseUrl}/Issue/GetAllChildIssues/${id}`;
    return this.http.get(url).pipe(
      map((response) => {
        return response as Issue[];
      })
    );
  }
  updateIssue(updatedIssue: AddIssueDTO): Observable<AddIssueDTO> {
    const url = `${this.baseUrl}/Issue/UpdateIssue`;
    return this.http.post(url, updatedIssue).pipe(
      map((response) => {
        return response as AddIssueDTO;
      })
    );
  }
}
