import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Project } from '../../Models/Project';
import { IssueService } from '../../Services/issue-service';
import { SprintService } from '../../Services/sprint-service';
import { Sprint } from '../../Models/Sprint';
import { EMPTY, Observable, concatMap } from 'rxjs';
import { Issue } from '../../Models/Issue';
import { AddIssueDTO } from '../../Models/DTO/AddIssueDTO';
import { DatePipe } from '@angular/common';
import { Data } from '@angular/router';
import { User } from '../../Models/User';
import { ProjectService } from '../../Services/project-service';
import { PriorityLevel } from '../../Models/PriorityLevel';
import { IssueLabel } from '../../Models/IssueLabel';

@Component({
  selector: 'app-add-issue',
  templateUrl: './add-issue.component.html',
  styleUrls: ['./add-issue.component.scss'],
})
export class AddIssueComponent implements OnInit {
  @Output() closeModalEvent = new EventEmitter();
  @Input() selectedProject!: Project;
  @Output() submitEvent = new EventEmitter<Issue[]>();
  @Input() parentIssue: Issue | undefined;
  labels: IssueLabel[] = [];
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
  priority: PriorityLevel = 0;
  labelId = '';
  selectedPriorityIndex: number = 0;
  priorities = [
    {
      value: PriorityLevel.Low,
      name: 'Low',
      color: 'green',
      icon: 'arrow_downward',
    },
    {
      value: PriorityLevel.Medium,
      name: 'Medium',
      color: '#DAA520',
      icon: 'arrow_right_alt',
    },
    {
      value: PriorityLevel.High,
      name: 'High',
      color: 'orange',
      icon: 'arrow_upward',
    },
    {
      value: PriorityLevel.Critical,
      name: 'Critical',
      color: 'red',
      icon: 'error',
    },
  ];
  constructor(
    private sprintService: SprintService,
    private issueService: IssueService,
    private projectService: ProjectService,
    private dataPipe: DatePipe
  ) {}
  close() {
    this.closeModalEvent.emit();
  }

  async onSubmit() {
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
      AssigneEmail:
        this.users.find((user) => user.fullName === this.assigneEmail)?.email ||
        '',
      ReporterEmail:
        this.users.find((user) => user.fullName === this.reporterEmail)
          ?.email || '',
      IssueStatus: this.issueStatus,
      IssueType: this.issueType,
      ParentIssueId:
        this.parentIssue?.id || '3fa85f64-5717-4562-b3fc-2c963f66afa6',
      LabelId: Number(this.labelId),
      Priority: this.priorities[this.selectedPriorityIndex].value,
    };
    try {
      const issues = await this.issueService.addIssue(this.issue).toPromise();
      console.log(issues);
      this.submitEvent.emit(issues);
    } catch (err) {
      console.log(err);
    }
    
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
    this.projectService.getAllLabelsForProject().subscribe((labels) => {
      this.labels = labels;
    });
  }
  priorityChange(newIndex: number) {
    this.selectedPriorityIndex = newIndex;
  }
}
