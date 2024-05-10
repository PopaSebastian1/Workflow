import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IssueService } from '../../Services/issue-service';
import { Issue } from '../../Models/Issue';
import { DatePipe } from '@angular/common';
import { Sprint } from '../../Models/Sprint';
import { ProjectService } from '../../Services/project-service';
import { AddIssueDTO } from '../../Models/DTO/AddIssueDTO';
@Component({
  selector: 'app-issue-details',
  templateUrl: './issue-details.component.html',
  styleUrls: ['./issue-details.component.scss'],
})
export class IssueDetailsComponent implements OnInit {
  showTable: boolean = true;
  issue: Issue | undefined;
  editedIssue?: Issue; // Create a separate editableIssue object
  formattedCreatedAt: string | undefined;
  formattedDueDate: string | undefined;
  sprints?: Sprint[];
  constructor(
    private route: ActivatedRoute,
    private issueService: IssueService,
    private datePipe: DatePipe,
    private projectService: ProjectService,
  ) {}

  toggleTable() {
    this.showTable = !this.showTable;
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
    this.projectService.getAllSprintsForProject().subscribe(sprints => {
      this.sprints = sprints;
    });
    if (id) {
      this.getIssue(id);
    }
  }
  
  getIssue(id: string): void {
    this.issueService.getIssue(id).subscribe((issue) => {
      this.issue = issue;
      this.editedIssue = { ...issue };
      this.formattedCreatedAt =
        this.datePipe.transform(this.issue.createdAt, 'yyyy-MM-dd') ||
        undefined;
      this.formattedDueDate = this.issue.dueDate
        ? this.datePipe.transform(this.issue.dueDate, 'yyyy-MM-dd') ||
          undefined
        : undefined;
  
      console.log(this.issue);
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
  
      this.issueService.updateIssue(issueToUpdate).subscribe((updatedIssue) => {
        console.log('Issue updated successfully', updatedIssue);
      }, (error) => {
        console.error('Error updating issue', error);
      });
    }
  }
}
