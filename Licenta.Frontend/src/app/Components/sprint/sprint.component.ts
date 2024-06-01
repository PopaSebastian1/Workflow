import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { ProjectService } from '../../Services/project-service';
import { Project } from '../../Models/Project';
import { Sprint } from '../../Models/Sprint';
import { SprintService } from '../../Services/sprint-service';
import { combineLatest } from 'rxjs';
import { map } from 'rxjs/operators';
import { SprintStatus } from '../../Models/SprintStatus';
import { Issue } from '../../Models/Issue';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-sprint',
  templateUrl: './sprint.component.html',
  styleUrls: ['./sprint.component.scss'],
})
export class SprintComponent implements OnInit {
  @Input() sprint: Sprint | null = null;
  showIssues: boolean = false;
  @Output() sprintStatusChanged= new EventEmitter();
  issues: MatTableDataSource<Issue>;
  displayedColumns: string[] = ['title', 'status', 'type'];
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatPaginator) set matPaginator(mp: MatPaginator) {
    this.paginator = mp;
    this.issues.paginator = this.paginator;
  }
  constructor(
    private projectService: ProjectService,
    private sprintService: SprintService
  ) {
    this.issues = new MatTableDataSource();
  }

  ngOnInit(): void {
    console.log(this.sprint);
    this.sprintService.getAllIssuesBySprint(this.sprint!.id).subscribe((issues) => {
      this.issues.data = issues;
      this.issues.paginator = this.paginator;
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

  calculateProgress(issues: Issue[]): number {
    const doneTasks = issues.filter(
      (issue) => issue.issueStatus?.name == 'Done'
    ).length;
    const totalTasks = issues.length;
    return totalTasks > 0 ? doneTasks / totalTasks : 0;
  }

  toggleIssues() {
    this.showIssues = !this.showIssues;
  }
  convertIssueType(nr:number)
  {
    if(nr==2)
    {
      return "Story"
    }
    if(nr==6)
    {
      return "Bug"
    }
    else
    {
      return "Task"
    }
  }
  

  calculateTimelineWidth(): string {
    if (!this.sprint || !this.sprint.startDate || !this.sprint.endDate) {
      return '0';
    }

    const startDate = new Date(this.sprint.startDate);
    const endDate = new Date(this.sprint.endDate);

    const totalDays =
      (endDate.getTime() - startDate.getTime()) / (1000 * 60 * 60 * 24);

    // Poți ajusta acest factor de scalare pentru a regla lungimea liniei de timp în funcție de spațiul disponibil în designul tău
    const scaleFactor = 5;

    return `${totalDays * scaleFactor}px`;
  }
  changeSprintStatus(status: number)
  {
    if(this.sprint)
    {
      this.sprintService.updateSprintStatus(this.sprint.id,status).subscribe((response)=>{
      });
      this.sprintStatusChanged.emit();
    }
  }
}
