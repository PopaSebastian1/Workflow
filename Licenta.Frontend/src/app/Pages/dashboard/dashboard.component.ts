import { Component, OnInit } from '@angular/core';
import { ProjectService } from '../../Services/project-service';
import { IssueService } from '../../Services/issue-service';
import { Issue } from '../../Models/Issue';
import { SprintService } from '../../Services/sprint-service';
import { Sprint } from '../../Models/Sprint';
import { Project } from '../../Models/Project';
import { User } from '../../Models/User';
import { DataService } from '../../Services/data-service';
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent implements OnInit {
  issues: Issue[] = [];
  project: Project | null = null;
  sprints: Sprint[] = [];
  currentUser : User | null = null;
  userIssues: Issue[] = [];
  pieChartsData: number[][] = [];
  pieChartsLabels: string[][] = [];
  chartTypes: string[] = [];
  isDropdownOpen = false;
  titles: string[] = [];

  constructor(
    private projectService: ProjectService,
    private issueService: IssueService,
    private sprintService: SprintService,
    private dataService: DataService
  ) {}

  ngOnInit(): void {
    this.currentUser = this.dataService.getUserData();
    if (this.projectService.getSelectedProject()) {
      this.project = this.projectService.getSelectedProject();
    }
    if (
      this.project &&
      this.issueService.getIssuesByProjectId(this.project.projectId)
    ) {
      this.issueService
      .getIssuesByProjectId(this.project.projectId)
      .subscribe((issues) => {
        this.issues = issues;
        this.userIssues=this.issues.filter((issue) => {
          return issue.assignee.email === this.currentUser?.email;
        });
        this.updateChartDataByStatus();
      });
    }
    if (
      this.project &&
      this.sprintService.getSprintsByProjectId(this.project.projectId)
    ) {
      this.sprintService
      .getSprintsByProjectId(this.project.projectId)
      .subscribe((sprints) => {
        this.sprints = sprints;
        this.loadIssuesforSprints();
      });
    }
    this.userIssues=this.issues.filter((issue) => {
      return issue.assignee.email === this.currentUser?.email;
    });
  }
  
  loadIssuesforSprints(): void {
    if (this.sprints.length > 0) {
      this.sprints.forEach((sprint) => {
        this.sprintService
        .getAllIssuesBySprint(sprint.id)
          .subscribe((issues) => {
            sprint.issues = issues;
          });
      });
    }
  }

  toggleDropdown() {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  updateChartDataByStatus(): void {
    const counts = {
      Done: 0,
      InProgress: 0,
      ToDo: 0,
    };

    this.issues.forEach((issue) => {
      switch (issue.issueStatus.name) {
        case 'Done':
          counts.Done++;
          break;
        case 'In Progress':
          counts.InProgress++;
          break;
        case 'To Do':
          counts.ToDo++;
          break;
      }
    });

    this.pieChartsData.push([counts.Done, counts.InProgress, counts.ToDo]);
    this.pieChartsLabels.push(['Done', 'In Progress', 'To Do']);
    this.chartTypes.push('pie');
    this.titles.push('Issues by Status');
  }

  updateChartDataByAssignee(): void {
    this.projectService.getAllMembers().subscribe((members) => {
      const assigneeCounts = members.reduce((counts, user) => {
        counts[user.fullName] = 0;
        return counts;
      }, {} as { [key: string]: number });

      this.issues.forEach((issue) => {
        if (
          issue.assignee &&
          assigneeCounts.hasOwnProperty(issue.assignee.fullName)
        ) {
          assigneeCounts[issue.assignee.fullName]++;
        }
      });

      this.pieChartsLabels.push(Object.keys(assigneeCounts));
      this.pieChartsData.push(Object.values(assigneeCounts));
      this.chartTypes.push('pie');
      this.titles.push('Issues by Assignee');
    });
  }
  updateChartIssuesBySprint(): void {
    const sprintCounts = this.sprints.map((sprint) => {
      return {
        name: sprint.name,
        count: sprint.issues.length
      };
    });
  
    this.pieChartsLabels.push(sprintCounts.map(sprint => sprint.name));
    this.pieChartsData.push(sprintCounts.map(sprint => sprint.count));
    this.chartTypes.push('pie');
    this.titles.push('Issues by Sprint');
  }
  updateChartDataByResolutionTime(): void {
    const counts = {
      '<1 day': 0,
      '1-3 days': 0,
      '3-7 days': 0,
      '>7 days': 0,
    };
  
    this.issues.forEach((issue) => {
      const resolutionTime = issue.estimatedTime/8;
      if (resolutionTime < 1) {
        counts['<1 day']++;
      } else if (resolutionTime <= 3) {
        counts['1-3 days']++;
      } else if (resolutionTime <= 7) {
        counts['3-7 days']++;
      } else {
        counts['>7 days']++;
      }
    });
  
    this.pieChartsData.push([counts['<1 day'], counts['1-3 days'], counts['3-7 days'], counts['>7 days']]);
    this.pieChartsLabels.push(['<1 day', '1-3 days', '3-7 days', '>7 days']);
    this.chartTypes.push('bar');
    this.titles.push('Issues by Resolution Time');
  }
  
  
  updateChartDataByPerformance(): void {
    const counts = {
      underTime: 0,
      onTime: 0,
      overTime: 0,
    };

    this.issues.forEach((issue) => {
      if (issue.loggedTime < issue.estimatedTime) {
        counts.underTime++;
      } else if (issue.loggedTime === issue.estimatedTime) {
        counts.onTime++;
      } else {
        counts.overTime++;
      }
    });

    this.pieChartsData.push([counts.underTime, counts.onTime, counts.overTime]);
    this.pieChartsLabels.push(['Under Time', 'On Time', 'Over Time']);
    this.chartTypes.push('pie');
    this.titles.push('Performance by Estimated Time');
  }


  onRemoveChart(index:number)
  {
    if (this.chartTypes[index] === 'pie') {
      this.pieChartsData.splice(index, 1);
      this.pieChartsLabels.splice(index, 1);
      this.chartTypes.splice(index, 1);
      this.titles.splice(index, 1);
    }
  }
  
}
