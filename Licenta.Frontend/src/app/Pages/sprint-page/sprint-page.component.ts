import { Component, OnInit } from '@angular/core';
import { ProjectService } from '../../Services/project-service';
import { SprintService } from '../../Services/sprint-service';
import { combineLatest } from 'rxjs';
import { Sprint } from '../../Models/Sprint';
import { Project } from '../../Models/Project';

@Component({
  selector: 'app-sprint-page',
  templateUrl: './sprint-page.component.html',
  styleUrls: ['./sprint-page.component.scss']
})
export class SprintPageComponent implements OnInit {
  project: Project | null = null;
  sprintsProject: Sprint[] = [];  // Initialize as empty array
  filteredSprints: Sprint[] = [];  // Initialize as empty array
  issues: boolean = false;
  isGridView = false;
  filterOption = 'all';
  sortOption = 'startDate';
  isAddSprintModalOpen = false;

  constructor(
    private projectService: ProjectService,
    private sprintService: SprintService
  ) {}

  ngOnInit(): void {
    this.project = this.projectService.getSelectedProject();
    if (this.project != null) {
      this.sprintService
        .getSprintsByProjectId(this.project.projectId)
        .subscribe((sprints) => {
          this.sprintsProject = sprints;
          this.loadIssuesForSprint();
          this.applyFiltersAndSort();
          console.log(this.sprintsProject);
        });
    }
  }
  openAddSprintModal(): void {
    this.isAddSprintModalOpen = true;
  }

  closeAddSprintModal(): void {
    this.isAddSprintModalOpen = false;
  }
  toggleViewMode() {
    this.isGridView = !this.isGridView;
  }

  loadIssuesForSprint(): void {
    const issuesObservables = this.sprintsProject.map((sprint) => {
      return this.sprintService.getAllIssuesBySprint(sprint.id);
    });

    combineLatest(issuesObservables).subscribe((issuesArrays) => {
      issuesArrays.forEach((issues, index) => {
        this.sprintsProject[index].issues = issues;
      });
      this.applyFiltersAndSort();
    });
  }

  onFilterChange(event: Event): void {
  const selectElement = event.target as HTMLSelectElement;
  this.filterOption = selectElement.value;
  this.applyFiltersAndSort();
  }

  onSortChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    this.sortOption = selectElement.value;
    this.applyFiltersAndSort();
  }
  applyFiltersAndSort(): void {
    if (!this.sprintsProject) return;

    let filtered = this.sprintsProject;
    console.log(filtered);

    // Apply filter
    if (this.filterOption === 'Completed') {
      filtered = filtered.filter(sprint => sprint.status === 2);
    } else if (this.filterOption === 'Active') {
      filtered = filtered.filter(sprint => sprint.status === 1);
    } else if(this.filterOption==="Planned"){
      filtered = filtered.filter(sprint => sprint.status === 0);
    }
    else if(this.filterOption==="all"){
      filtered = this.sprintsProject;
    }

    // Apply sort
    if (this.sortOption === 'startDate') {
      filtered.sort((a, b) => new Date(a.startDate).getTime() - new Date(b.startDate).getTime());
    } else if (this.sortOption === 'endDate') {
      filtered.sort((a, b) => new Date(b.endDate).getTime() - new Date(a.endDate).getTime());
    } else if (this.sortOption === 'remainingDays') {
      filtered.sort((a, b) => this.calculateRemainingDays(a.endDate) - this.calculateRemainingDays(b.endDate));
    }

    this.filteredSprints = filtered;
  }

  calculateDuration(startDate: Date, endDate: Date): number {
    const start = new Date(startDate);
    const end = new Date(endDate);
    return Math.floor((end.getTime() - start.getTime()) / (1000 * 60 * 60 * 24));
  }

  calculateRemainingDays(endDate: Date): number {
    const today = new Date();
    const end = new Date(endDate);
    return Math.floor((end.getTime() - today.getTime()) / (1000 * 60 * 60 * 24));
  }


}
