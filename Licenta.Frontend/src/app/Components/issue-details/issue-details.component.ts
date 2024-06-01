import { Component, EventEmitter, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IssueService } from '../../Services/issue-service';
import { Issue } from '../../Models/Issue';
import { DatePipe } from '@angular/common';
import { Sprint } from '../../Models/Sprint';
import { ProjectService } from '../../Services/project-service';
import { AddIssueDTO } from '../../Models/DTO/AddIssueDTO';
import { User } from '../../Models/User';
import { Project } from '../../Models/Project';
import { Router } from '@angular/router';
@Component({
  selector: 'app-issue-details',
  templateUrl: './issue-details.component.html',
  styleUrls: ['./issue-details.component.scss'],
})
export class IssueDetailsComponent implements OnInit {
  showTable: boolean = true;
  issue: Issue | undefined;
  editedIssue?: Issue;
  formattedCreatedAt: string | undefined;
  formattedDueDate: string | undefined;
  sprints?: Sprint[];
  projectUsers: User[] = [];
  showAddIssue = false;
  selectedProject: Project | null = null;
  allIssues: Issue[] = [];
  selectedChildIssue: Issue | 'newIssue' = 'newIssue';
  estimatedTimeUnit: string= 'd';
  loggedTimeUnit: string = 'd';
  
  constructor(
    private route: ActivatedRoute,
    private issueService: IssueService,
    private datePipe: DatePipe,
    private projectService: ProjectService,
    
  ) {}

  toggleTable() {
    this.showTable = !this.showTable;
  }
  closeModal() {
    this.showAddIssue = false;
  }

  getProgressBarColor() {
    const progress = this.getProgress();
    if (progress == 0) {
      return 'progress-low';
    }
    if (this.issue?.loggedTime === 0 && this.issue.estimatedTime === 0) {
      return 'progress-none';
    }
    if (progress < 50) {
      return 'progress-low'; // pentru roșu
    } else if (progress < 100) {
      return 'progress-medium'; // pentru galben
    } else {
      return 'progress-high'; // pentru verde
    }
  }

  getProgress() {
    if (!this.issue) {
      return 0;
    }
    return (this.issue.loggedTime / this.issue.estimatedTime) * 100;
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    this.projectService.getAllSprintsForProject().subscribe((sprints) => {
      this.sprints = sprints;
    });
    if (id) {
      this.getIssue(id);
    }
    this.projectService.getAllMembers().subscribe((users) => {
      this.projectUsers = users;
    });
    this.selectedProject = this.projectService.getSelectedProject();
    if (this.selectedProject) {
      this.issueService
      .getIssuesByProjectId(this.selectedProject.projectId.toString())
      .subscribe((issues: Issue[]) => {
        this.allIssues = issues.filter(
          (issue) => issue.id !== this.editedIssue?.id && !this.issue?.childIssues.some(child => child.id === issue.id)
        );
      });
    }
    console.log(this.selectedProject);
  }
  addExistingIssueAsChild(childIssue: Issue) {
    if (this.editedIssue) {
    this.issueService.addChildIssue(this.editedIssue.id, childIssue.id).subscribe(() => {
        if (this.editedIssue) {
          this.getIssue(this.issue!.id);
          
        }
        window.location.reload();

      });
    }
  }
  getIssue(id: string): void {
    this.issueService.getIssue(id).subscribe((issue) => {
      this.issue = issue;
      this.editedIssue = { ...issue };
      console.log(this.editedIssue);
      this.formattedCreatedAt =
        this.datePipe.transform(this.issue.createdAt, 'yyyy-MM-dd') ||
        undefined;
      this.formattedDueDate = this.issue.dueDate
        ? this.datePipe.transform(this.issue.dueDate, 'yyyy-MM-dd') || undefined
        : undefined;
    });
  }

  saveChanges(): void {
    if (this.editedIssue && this.issue) {
      this.issue = { ...this.editedIssue };

      const issueToUpdate = new AddIssueDTO();
      issueToUpdate.Id = this.issue.id;
      issueToUpdate.Title = this.issue.title;
      issueToUpdate.Description = this.issue.description;
      issueToUpdate.CreatedAt = this.issue.createdAt;
      issueToUpdate.DueDate = this.issue.dueDate;
      issueToUpdate.EstimatedTime = this.issue.estimatedTime;
      issueToUpdate.LoggedTime = this.issue.loggedTime;
      issueToUpdate.ProjectId = this.issue.projectId;
      issueToUpdate.SprintId = this.issue.sprintId;
      issueToUpdate.AssigneEmail = this.issue.assignee.email;
      issueToUpdate.ReporterEmail = this.issue.reporter.email;
      issueToUpdate.IssueStatus = this.issue.issueStatus.name;
      issueToUpdate.IssueType = this.issue.issueType.name;
      issueToUpdate.ParentIssueId='3fa85f64-5717-4562-b3fc-2c963f66afa6',

      this.issueService.updateIssue(issueToUpdate).subscribe(
        (updatedIssue) => {
          console.log('Issue updated successfully', updatedIssue);
        },
        (error) => {
          console.error('Error updating issue', error);
        }
      );
    }
  }
  convertTimeToHours(time: number, unit: string): number {
    switch(unit) {
      case 'days':
        return time * 8; // Convert days to hours
      // Add more cases as needed
      default:
        return time;
    }
  }
  compareUsers(user1: User, user2: User): boolean {
    return user1 && user2 ? user1.email === user2.email : user1 === user2;
  }
  handleRemoveIssueEvent(issueId: string) {
    this.issueService.removeChildIssue(this.editedIssue!.id, issueId).subscribe(() => {
      if (this.editedIssue) {
        this.getIssue(this.editedIssue.id);
      }
      window.location.reload();
    });
  }
}
