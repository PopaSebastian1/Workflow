import { Component, OnInit } from '@angular/core';
import { Issue } from '../../Models/Issue';
import { EMPTY, Observable } from 'rxjs';
import { IssueService } from '../../Services/issue-service';
import { Project } from '../../Models/Project';
import { ProjectService } from '../../Services/project-service';
import { IssueStatus } from '../../Models/IssueStatus';
import { IssueType } from '../../Models/IssueType';
@Component({
  selector: 'app-issue-list',
  templateUrl: './issue-list.component.html',
  styleUrls: ['./issue-list.component.scss'],
})
export class IssueListComponent implements OnInit {
applyFilter(arg0: string) {
throw new Error('Method not implemented.');
}
  displayedColumns: string[] = [
    'number',
    'title',
    'status',
    'type',
    'createdOn',
    'reporter',
    'assignee',
  ];
  showCreateProject: boolean = false;
  
  issues: Observable<Issue[]> = EMPTY;
  selectedProject: Project | null = null;
  getTypeColor(type: IssueType) {
    if (type.name == 'Story') {
      return 'green';
    } else {
      return 'red';
    }
  }
  getStatusColor(arg0: IssueStatus) {
    if (arg0.name == 'Done') {
      return 'green';
    }
    if (arg0.name == 'In Progress') {
      return 'yellow';
    }
    if (arg0.name == 'To Do') {
      return 'red';
    } else {
      return 'white';
    }
  }
  constructor(
    private issueService: IssueService,
    private projectService: ProjectService
  ) {}
  
  setSelectedProject() {
    this.selectedProject = this.projectService.getSelectedProject();
  }
  
  closeCreateProject() {
    this.showCreateProject = !this.showCreateProject;
  }

  openAddIssue() {
  this.showCreateProject = true;
  }
  ngOnInit(): void {
    this.setSelectedProject();
    if (this.selectedProject) {
      this.issues = this.issueService.getIssuesByProjectId(
        this.selectedProject.projectId.toString()
      );
      console.log(this.issues);
    }
  }
}
