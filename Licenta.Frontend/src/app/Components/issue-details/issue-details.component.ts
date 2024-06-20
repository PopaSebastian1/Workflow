import { ChangeDetectorRef, Component, EventEmitter, OnInit } from '@angular/core';
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
import { PriorityLevel } from '../../Models/PriorityLevel';
import { IssueLabel } from '../../Models/IssueLabel';
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
  estimatedTimeUnit: string = 'd';
  loggedTimeUnit: string = 'd';
  labelMode: boolean = false;
  allOtherIssues: Issue[]= [];
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
  labels: IssueLabel[] = [];
  constructor(
    private route: ActivatedRoute,
    private issueService: IssueService,
    private datePipe: DatePipe,
    private projectService: ProjectService,
    private cdr: ChangeDetectorRef
  ) { 
  }

  toggleTable() {
    this.showTable = !this.showTable;
  }
  closeModal() {
    this.showAddIssue = false;
  }
  closeLabel() {
    this.labelMode = false;
  }
  openAddLabelDialog(): void {
    this.labelMode = true;
  }
  addedLabel() {
    this.projectService.getAllLabelsForProject().subscribe((labels) => {
      this.labels = labels;
    });
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
  getIssueLabels(): void {
    this.projectService.getAllLabelsForProject().subscribe((labels) => {
      this.labels = labels;
      console.log('Labels', labels);
    });
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
      this.getIssueLabels();
      this.issueService.getIssuesByProjectId(this.selectedProject.projectId.toString())
        .subscribe((issues: Issue[]) => {
          console.log('All issues before filtering:', issues); // Debug
          this.allIssues = issues;
          this.filterIssues();
          this.cdr.detectChanges(); // Forțare detectare schimbări
        });
    }
    console.log(this.selectedProject);
  }
  
  addExistingIssueAsChild(childIssue: Issue) {
    if (this.editedIssue) {
      this.issueService
        .addChildIssue(this.editedIssue.id, childIssue.id)
        .subscribe(() => {
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
      console.log('Edited issue:', this.editedIssue); // Debug
      this.formattedCreatedAt =
        this.datePipe.transform(this.issue.createdAt, 'yyyy-MM-dd') || undefined;
      this.formattedDueDate = this.issue.dueDate
        ? this.datePipe.transform(this.issue.dueDate, 'yyyy-MM-dd') || undefined
        : undefined;
  
      // Reapply the filtering after the edited issue is set
      this.filterIssues();
      this.cdr.detectChanges(); // Forțare detectare schimbări
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
      (issueToUpdate.ParentIssueId = '3fa85f64-5717-4562-b3fc-2c963f66afa6'),
        (issueToUpdate.Priority = this.issue.priority);
      issueToUpdate.LabelId = this.issue.labelId;

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
    switch (unit) {
      case 'days':
        return time * 8; // Convert days to hours
      // Add more cases as needed
      default:
        return time;
    }
  }
  changeIssueLabel(event: any): void {
    this.issue!.labelId = event.value;
    this.editedIssue!.labelId = event.value;
    // alte operații necesare
  }
  changeIssuePriority(event: any): void {
    this.issue!.priority = event.value;
    this.editedIssue!.priority = event.value;
    // alte operații necesare
  }
  compareUsers(user1: User, user2: User): boolean {
    return user1 && user2 ? user1.email === user2.email : user1 === user2;
  }
  handleRemoveIssueEvent(issueId: string) {
    this.issueService
      .removeChildIssue(this.editedIssue!.id, issueId)
      .subscribe(() => {
        if (this.editedIssue) {
          this.getIssue(this.editedIssue.id);
        }
        window.location.reload();
      });
  }
  filterIssues(): void {
    if (this.editedIssue) {
      this.allIssues = this.allIssues.filter(
        (issue) => issue.id !== this.editedIssue!.id &&
                   !this.issue?.childIssues?.some((child) => child.id === issue.id)
      );
    }
    console.log('Filtered issues:', this.allIssues); // Debug
    this.cdr.detectChanges(); // Forțare detectare schimbări
  }
}
