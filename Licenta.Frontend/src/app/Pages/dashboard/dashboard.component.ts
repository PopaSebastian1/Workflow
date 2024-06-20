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
  showStatistics: boolean = false;
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
      '<4 hours': 0,
      '4-8 hours': 0,
      '8-12 hours': 0,
      '>12 hours': 0,
    };
  
    this.issues.forEach((issue) => {
      const resolutionTime = issue.estimatedTime; // Assuming estimatedTime is in hours
      if (resolutionTime < 4) {
        counts['<4 hours']++;
      } else if (resolutionTime <= 8) {
        counts['4-8 hours']++;
      } else if (resolutionTime <= 12) {
        counts['8-12 hours']++;
      } else {
        counts['>12 hours']++;
      }
    });
  
    this.pieChartsData.push([counts['<4 hours'], counts['4-8 hours'], counts['8-12 hours'], counts['>12 hours']]);
    this.pieChartsLabels.push(['<4 hours', '4-8 hours', '8-12 hours', '>12 hours']);
    this.chartTypes.push('bar');
    this.titles.push('Issues by Resolution Time in Hours');
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
  updateChartDataByPriority(): void {
    const counts = {
      Critical: 0,
      High: 0,
      Low: 0,
      Medium: 0,
    };
    this.issues.forEach((issue) => {
      switch (issue.priority) {
        case 3:
          counts.Critical++;
          break;
        case 2:
          counts.High++;
          break;
        case 0:
          counts.Low++;
          break;
        case 1:
          counts.Medium++;
          break;
      }
    });
    this.pieChartsData.push([counts.Critical, counts.High, counts.Low, counts.Medium]);
    this.pieChartsLabels.push(['Critical', 'High', 'Low', 'Medium']);
    this.chartTypes.push('pie');
    this.titles.push('Issues by Priority');
  }
  updateChartByLabel(): void {
    this.projectService.getAllLabelsForProject().subscribe((labels) => {
      // Inițializează obiectul de contorizare cu un identificator implicit pentru "None"
      const labelCounts = labels.reduce((counts, label) => {
        counts[label.id] = 0;
        return counts;
      }, { 'none': 0 } as { [key: string]: number });
  
      // Iterează prin toate problemele
      this.issues.forEach((issue) => {
        const labelId = issue.labelId || 'none'; // Utilizează 'none' dacă issue.labelId este falsy
        if (labelCounts.hasOwnProperty(labelId)) {
          labelCounts[labelId]++;
        }
      });
  
      // Map label IDs to label names, including "None"
      const labelNames = labels.reduce((acc, label) => {
        acc[label.id] = label.name;
        return acc;
      }, { 'none': 'None' } as { [key: string]: string });
  
      // Actualizează datele pentru grafic
      this.pieChartsLabels.push(Object.keys(labelCounts).map(key => labelNames[key]));
      this.pieChartsData.push(Object.values(labelCounts));
      this.chartTypes.push('pie');
      this.titles.push('Issues by Label');
    });
  }
  onRemoveChart(index:number)
  {
    if (this.chartTypes[index]) {
      this.pieChartsData.splice(index, 1);
      this.pieChartsLabels.splice(index, 1);
      this.chartTypes.splice(index, 1);
      this.titles.splice(index, 1);
    }
  }
  toggleStatistics(): void {
    this.showStatistics = !this.showStatistics;
  }
}
