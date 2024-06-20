import { Component, OnInit, ViewChild } from '@angular/core';
import { Issue } from '../../Models/Issue';
import { EMPTY, Observable, filter, map } from 'rxjs';
import { IssueService } from '../../Services/issue-service';
import { Project } from '../../Models/Project';
import { ProjectService } from '../../Services/project-service';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { PriorityLevel } from '../../Models/PriorityLevel';
import { MatDialog } from '@angular/material/dialog';
import { IssueLabel } from '../../Models/IssueLabel';
import { MatSort, Sort, MatSortModule } from '@angular/material/sort';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { AddIssueComponent } from '../../Components/add-issue/add-issue.component';
import { ConfirmDeleteDialogComponent } from '../../Components/confirm-delete-dialog/confirm-delete-dialog.component';

@Component({
  selector: 'app-issue-list',
  templateUrl: './issue-list.component.html',
  styleUrls: ['./issue-list.component.scss'],
})
export class IssueListComponent implements OnInit {
  displayedColumns: string[] = [
    'number',
    'title',
    'priority',
    'status',
    'type',
    'createdOn',
    'dueDate',
    'assignee',
    'actions',
  ];
  @ViewChild(AddIssueComponent) addIssueComponent!: AddIssueComponent;
  showCreateProject: boolean = false;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  dataSource: MatTableDataSource<Issue> = new MatTableDataSource<Issue>();
  priorityLevel = PriorityLevel;
  issues: Observable<Issue[]> = EMPTY;
  selectedProject: Project | null = null;
  labels: IssueLabel[] = [];
  appliedPriorities: string[] = [];
  appliedStatuses: string[] = [];
  appliedTypes: string[] = [];
  appliedLabels: number[] = [];
  originalData: Issue[] = [];
  statusList: string[] = ['Open', 'In Progress', 'Closed']; // Add your statuses here
  typeList: string[] = ['Story', 'Task', 'Bug'];
  sortOption: string = 'createdAtDesc';

  constructor(
    private issueService: IssueService,
    private projectService: ProjectService,
    private dialog: MatDialog
  ) {}

  removeIssue(issueId: any) {
    const dialogRef = this.dialog.open(ConfirmDeleteDialogComponent, {
      width: '250px',
      data: { id: issueId }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        console.log('Delete clicked'+ result);
        this.projectService.removeIssueFromProject(issueId).subscribe(() => {
          if (this.selectedProject) {
            this.issues = this.issueService.getIssuesByProjectId(
              this.selectedProject.projectId.toString()
            );
            
            this.issues.subscribe((data) => {
              this.dataSource.data = data;
              this.dataSource.paginator = this.paginator;
              this.dataSource.sort = this.sort;
            });
          }
        });
      }
    });
  }

  getProjectLabels() {
    this.projectService.getAllLabelsForProject().subscribe((data) => {
      this.labels = data;
      this.labels.push({ id: -1, name: 'No label', color: '', projectId: '0' });
    });
  }

  setSelectedProject() {
    this.selectedProject = this.projectService.getSelectedProject();
  }

  closeCreateProject() {
    this.showCreateProject = !this.showCreateProject;
  }

  getLabelById(id: number) {
    return this.labels.find((label) => label.id == id);
  }

  newIssueAdded(issues: Issue[]) {
    this.dataSource.data = issues;
    this.dataSource.paginator = this.paginator;
    this.originalData = issues;
    this.filterData();
  }

  openAddIssue() {
    this.showCreateProject = true;
  }

  ngOnInit(): void {
    this.setSelectedProject();
    if (this.selectedProject) {
      this.issues = this.issueService.getIssuesByProjectId(
        this.selectedProject.projectId.toString()
      );
      this.appliedPriorities = this.projectService.getPriorities();
      this.appliedStatuses = ['1', '2', '3'];
      this.appliedTypes = ['2', '6', '7'];
      this.projectService.getAllLabelsForProject().subscribe((labels) => {
        this.appliedLabels = labels.map((label) => label.id);
        this.appliedLabels.push(-1);
      });

      this.getProjectLabels();
      this.issues.subscribe((data) => {
        this.dataSource.data = data;
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        this.originalData = data;
      });
    }
  }

  applyFilter(event: any, filterType: number) {
    if (event.checked) {
      this.appliedPriorities.push(filterType.toString());
    } else {
      const index = this.appliedPriorities.indexOf(filterType.toString());
      if (index >= 0) {
        this.appliedPriorities.splice(index, 1);
      }
    }

    this.filterData();
  }

  applyStatusFilter(event: MatCheckboxChange, status: string) {
    if (event.checked) {
      if (!this.appliedStatuses.includes(status)) {
        this.appliedStatuses.push(status);
      }
    } else {
      const index = this.appliedStatuses.indexOf(status);
      if (index >= 0) {
        this.appliedStatuses.splice(index, 1);
      }
    }

    this.filterData();
  }

  applyTypeFilter(event: MatCheckboxChange, type: string) {
    if (event.checked) {
      if (!this.appliedTypes.includes(type)) {
        this.appliedTypes.push(type);
      }
    } else {
      const index = this.appliedTypes.indexOf(type);
      if (index >= 0) {
        this.appliedTypes.splice(index, 1);
      }
    }

    this.filterData();
  }
  applyFilterLabel(event: MatCheckboxChange, labelId: number) {
    // Tratarea cazului special pentru lipsa etichetei
    if (labelId === -1) {
      if (event.checked) {
        if (!this.appliedLabels.includes(-1)) {
          this.appliedLabels.push(-1);
        }
      } else {
        const index = this.appliedLabels.indexOf(-1);
        if (index >= 0) {
          this.appliedLabels.splice(index, 1);
        }
      }
    } else {
      if (event.checked) {
        if (!this.appliedLabels.includes(labelId)) {
          this.appliedLabels.push(labelId);
        }
      } else {
        const index = this.appliedLabels.indexOf(labelId);
        if (index >= 0) {
          this.appliedLabels.splice(index, 1);
        }
      }
    }
    this.filterData();
  }

  filterData() {
    let filteredData = this.originalData;

    if (this.appliedPriorities.length >= 0) {
      filteredData = filteredData.filter((issue) =>
        this.appliedPriorities.includes(issue.priority.toString())
      );
    }

    if (this.appliedStatuses.length >= 0) {
      filteredData = filteredData.filter((issue) =>
        this.appliedStatuses.includes(issue.issueStatusId.toString())
      );
    }

    if (this.appliedTypes.length >= 0) {
      filteredData = filteredData.filter((issue) =>
        this.appliedTypes.includes(issue.issueTypeId.toString())
      );
    }

    if (this.appliedLabels.length >= 0) {
      filteredData = filteredData.filter((issue) => {
        const issueLabelId = issue.labelId != null ? issue.labelId : -1;
        // Includem și problemele fără etichetă dacă -1 este în appliedLabels
        return this.appliedLabels.includes(issueLabelId);
      });
    }

    this.dataSource.data = filteredData;
  }
  exportData() {
    var issues: string[] = [];
    this.dataSource.data.map((issue) => {
      issues.push(issue.id);
    });
    this.issueService.exportIssuesToCsv(issues).subscribe(
      (blob: Blob) => {
        const link = document.createElement('a');
        const url = window.URL.createObjectURL(blob);
        link.href = url;
        link.download = 'issues.csv';
        link.click();
        window.URL.revokeObjectURL(url);
      },
      (err) => {
        console.log(err);
      }
    );
  }

  sortData(sortField: string, sortOrder: 'asc' | 'desc') {
    const sortedData = this.dataSource.data.sort((a, b) => {
      if (sortField === 'assignee') {
        return sortOrder === 'asc'
          ? a.assignee.fullName.localeCompare(b.assignee.fullName)
          : b.assignee.fullName.localeCompare(a.assignee.fullName);
      } else {
        const aValue = a[sortField as keyof Issue];
        const bValue = b[sortField as keyof Issue];

        if (aValue! < bValue!) {
          return sortOrder === 'asc' ? -1 : 1;
        } else if (aValue! > bValue!) {
          return sortOrder === 'asc' ? 1 : -1;
        } else {
          return 0;
        }
      }
    });

    this.dataSource.data = sortedData;
  }
  onSortOptionChange() {
    this.filterData();
  }
  compare(
    a: number | string | Date,
    b: number | string | Date,
    isAsc: boolean
  ) {
    return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
  }
  announceSortChange(sort: Sort) {
    const data = this.dataSource.data.slice();
    if (!sort.active || sort.direction === '') {
      this.dataSource.data = data;
      return;
    }

    this.dataSource.data = data.sort((a, b) => {
      const isAsc = sort.direction === 'asc';
      switch (sort.active) {
        case 'createdOn':
          return this.compare(
            new Date(a.createdAt),
            new Date(b.createdAt),
            isAsc
          );
        case 'dueDate':
          return this.compare(new Date(a.dueDate), new Date(b.dueDate), isAsc);
        case 'reporter':
          return this.compare(a.reporter.fullName, b.reporter.fullName, isAsc);
        case 'assignee':
          return this.compare(a.assignee.fullName, b.assignee.fullName, isAsc);
        case 'priority':
          return this.compare(a.priority, b.priority, isAsc);
        case 'title':
          return this.compare(a.title, b.title, isAsc);
        case 'status':
          return this.compare(a.issueStatus.name, b.issueStatus.name, isAsc);
        case 'type':
          return this.compare(a.issueType.name, b.issueType.name, isAsc);
        default:
          return 0;
      }
    });
  }
}
