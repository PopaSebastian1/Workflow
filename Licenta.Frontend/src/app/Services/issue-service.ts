import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
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
  addIssue(issue: AddIssueDTO): Observable<Issue[]> {
    console.log(issue);
    const url = 'https://localhost:7261/Issue/AddIssue';
    return this.http.post(url, issue).pipe(
      map((response) => {
        return response as Issue[];
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
  addChildIssue(parentId: string, childId: string): Observable<void> {
    const url = `${this.baseUrl}/Issue/AddChildIssue/${parentId}/${childId}`;
    return this.http.post<void>(url, {}).pipe(
      map(() => {
        console.log('Child issue added successfully');
      }),
      catchError((error) => {
        console.error('Error adding child issue', error);
        return throwError(error);
      })
    );
  }
  removeChildIssue(parentId: string, childId: string): Observable<void> {
    const url = `${this.baseUrl}/Issue/RemoveChildIssue/${parentId}/${childId}`;
    return this.http.patch<void>(url, {}).pipe(
      map(() => {
        console.log('Child issue removed successfully');
      }),
      catchError((error) => {
        console.error('Error removing child issue', error);
        return throwError(error);
      })
    );
  }
  exportIssuesToCsv(ids: string[]): Observable<Blob> {
    const url = `${this.baseUrl}/Issue/ExportIssuesToCsv`;
    return this.http.post(url, ids, { responseType: 'blob' });
  }
}
