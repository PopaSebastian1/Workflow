import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Project } from '../../Models/Project';
import { IssueService } from '../../Services/issue-service';
import { SprintService } from '../../Services/sprint-service';
import { Sprint } from '../../Models/Sprint';
import { EMPTY, Observable } from 'rxjs';
import { Issue } from '../../Models/Issue';
import { AddIssueDTO } from '../../Models/DTO/AddIssueDTO';
import { DatePipe } from '@angular/common';
import { Data } from '@angular/router';
import { User } from '../../Models/User';
import { ProjectService } from '../../Services/project-service';
@Component({
  selector: 'app-add-issue',
  templateUrl: './add-issue.component.html',
  styleUrls: ['./add-issue.component.scss'],
})
export class AddIssueComponent implements OnInit {
  @Output() closeModalEvent = new EventEmitter();
  @Input() selectedProject!: Project;
  @Output() submitEvent= new EventEmitter();
  @Input() parentIssue: Issue | undefined;
  users: User[] = [];
  sprintsProject: Sprint[] | null = null;
  issue?: AddIssueDTO;
  title = '';
  description = '';
  createdAt = '';
  dueDate = '';
  estimatedTime = '';
  loggedTime = '';
  sprintId = '';
  assigneEmail = '';
  reporterEmail = '';
  issueStatus = '';
  issueType = '';
  showModal = true;

  constructor(
    private sprintService: SprintService,
    private issueService: IssueService,
    private projectService: ProjectService,
    private dataPipe: DatePipe
  ) {}
  close() {
    this.closeModalEvent.emit();
  }

  onSubmit() {
    this.issue = {
      Id: '3fa85f64-5717-4562-b3fc-2c963f66afa6',
      Title: this.title,
      Description: this.description,
      CreatedAt: new Date(this.createdAt),
      DueDate: new Date(this.dueDate),
      EstimatedTime: Number(this.estimatedTime),
      LoggedTime: Number(this.loggedTime),
      ProjectId: this.selectedProject.projectId,
      SprintId: this.sprintId,
      AssigneEmail: this.users.find((user) => user.fullName === this.assigneEmail)?.email || '',
      ReporterEmail: this.users.find((user)=> user.fullName === this.reporterEmail)?.email || '',
      IssueStatus: this.issueStatus,
      IssueType: this.issueType,
      ParentIssueId: this.parentIssue?.id || '3fa85f64-5717-4562-b3fc-2c963f66afa6',

    };

    this.issueService.addIssue(this.issue).subscribe({
      next: (issue) => {
        console.log(issue);
      },
      error: (err) => {
        console.log(err);
      },
    });
    this.submitEvent.emit();

    this.close();
  }
  ngOnInit(): void {
    this.sprintService
      .getSprintsByProjectId(this.selectedProject.projectId)
      .subscribe((sprints) => {
        this.sprintsProject = sprints;
      });
      this.projectService.getAllMembers().subscribe((users) => {
        this.users = users;
      });
  }

}
