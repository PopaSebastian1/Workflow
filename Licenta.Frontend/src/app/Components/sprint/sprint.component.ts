import { Component, OnInit } from '@angular/core';
import { ProjectService } from '../../Services/project-service';
import { Project } from '../../Models/Project';
import { Sprint } from '../../Models/Sprint';
import { SprintService } from '../../Services/sprint-service';
import { combineLatest } from 'rxjs';
import { map } from 'rxjs/operators';
@Component({
  selector: 'app-sprint',
  templateUrl: './sprint.component.html',
  styleUrls: ['./sprint.component.scss'],
})
export class SprintComponent implements OnInit {
  project: Project | null = null;
  sprintsProject: Sprint[] | null = null;
  issues: boolean = false;
  constructor(
    private projectService: ProjectService,
    private sprintService: SprintService
  ) {}

  ngOnInit(): void {
    this.project = this.projectService.getSelectedProject();
    console.log(this.project);
    if (this.project != null) {
      this.sprintService
        .getSprintsByProjectId(this.project.projectId)
        .subscribe((sprints) => {
          this.sprintsProject = sprints;
          console.log(this.sprintsProject);
          this.loadIssuesForSprint();
        });
    }
  }
  
  loadIssuesForSprint(): void {
    const issuesObservables = this.sprintsProject?.map((sprint) => {
      return this.sprintService.getAllIssuesBySprint(sprint.id);
    }) ?? [];
  
    combineLatest(issuesObservables).subscribe((issuesArrays) => {
      issuesArrays.forEach((issues, index) => {
        this.sprintsProject![index].issues = issues;
      });
      console.log(this.sprintsProject);
    });
  }
  calculateDuration(startDate: Date, endDate: Date): number {
    if (!startDate || !endDate) {
      console.error('Invalid dates provided to calculateDuration');
      return 0;
    }

    const start = new Date(startDate);
    const end = new Date(endDate);
    const millisecondsPerDay = 1000 * 60 * 60 * 24;
    const duration = Math.floor(
      (end.getTime() - start.getTime()) / millisecondsPerDay
    );
    return duration + 1; // Adăugăm +1 pentru a include data de sfârșit în durată
  }

  calculateRemainingDays(endDate: Date): number {
    if (!endDate) {
      console.error('Invalid date provided to calculateRemainingDays');
      return 0;
    }

    const today = new Date();
    const end = new Date(endDate);
    const remainingTime = end.getTime() - today.getTime();
    const remainingDays = Math.ceil(remainingTime / (1000 * 60 * 60 * 24));
    return remainingDays >= 0 ? remainingDays : 0;
  }

  calculateProgress(sprint: Sprint): number {
    console.log(sprint);
    if (!sprint || !sprint.issues) {
      return 0; // Sau altă valoare implicită corespunzătoare
    }
  
    const doneTasks = sprint.issues.filter(issue => issue.issueStatus?.name == 'Done').length;
    const totalTasks = sprint.issues.length;
    return totalTasks > 0 ? doneTasks / totalTasks : 0;
  }
  showIssues(sprint: any) {
    this.issues =!this.issues;
  }
}
