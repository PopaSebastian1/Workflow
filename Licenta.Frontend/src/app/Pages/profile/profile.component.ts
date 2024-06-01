import { AfterViewChecked, AfterViewInit, Component, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { User } from '../../Models/User';
import { DataService } from '../../Services/data-service';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Project } from '../../Models/Project';
import { Issue } from '../../Models/Issue';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss',
})
export class ProfileComponent implements AfterViewChecked{
  userProfile: User | null = null;
  profileForm: FormGroup;
  userId: string = '';
  loggedUser: User | null = null;
  projectIssuesMap: { [key: string]: Issue[] } = {};
  userProjects: Project[] = [];
  userIssues: Issue[] = [];
  commonProjects: Project[] = [];
  displayedColumnsProjects: string[] = ['name', 'description'];
  displayedColumns: string[] = ['title', 'description'];
  tableVisible: boolean[] = [];
  @ViewChildren(MatPaginator) paginators!: QueryList<MatPaginator>;
  projectIssueDataSources: MatTableDataSource<Issue>[] = [];
  constructor(
    private userService: DataService,
    private formBuilder: FormBuilder,
    private route: ActivatedRoute
  ) {
    this.profileForm = this.formBuilder.group({
      firstName: [''],
      lastName: [''],
      email: [''],
    });
  }
  
  splitIssuesByProject(issues: Issue[], projects: Project[]) {
    projects.forEach((project, index) => {
      const projectIssues = issues.filter((issue) => issue.projectId === project.projectId);
      this.projectIssuesMap[project.projectId] = projectIssues;
      this.projectIssueDataSources[index] = new MatTableDataSource(projectIssues);
    });
  }

  ngAfterViewChecked() {
    this.paginators.toArray().forEach((paginator, index) => {
      if (this.projectIssueDataSources[index].paginator != paginator) {
        this.projectIssueDataSources[index].paginator = paginator;
      }
    });
  }
  toggleTable(index: number) {
    this.tableVisible[index] = !this.tableVisible[index];
  }
  
  getUserData() {
    this.userProfile = this.userService.getUserData();
  }
  
  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.userId = params['id'];
    });
    this.getUserData();
    this.getUserProfile(this.userId);
    if (this.userService.getUserData()) {
      this.loggedUser = this.userService.getUserData();
    }
  }
  
  getUserProfile(id: string) {
    this.userService.getUserById(this.userId).subscribe((user: User) => {
      this.userProfile = user;
      this.profileForm = this.formBuilder.group({
        firstName: [this.userProfile?.firstName],
        lastName: [this.userProfile?.lastName],
        email: [this.userProfile?.email],
      });
      if (this.userProfile) {
        this.userService.getAllUserProject(this.userProfile?.email.toString()).subscribe((projects: Project[]) => {
          this.userProjects = projects;
          this.getCommonProjects();
        });
        this.userService.getAllUserIssues(this.userProfile?.id.toString()).subscribe((issues: Issue[]) => {
          this.userIssues = issues;
        });
      }
    });
  }

  getCommonProjects() {
    if (this.loggedUser && this.userProfile) {
      this.userService.getAllUserProject(this.loggedUser.email.toString()).subscribe((loggedUserProjects: Project[]) => {
        this.commonProjects = this.userProjects.filter(project => 
          loggedUserProjects.some(loggedUserProject => loggedUserProject.projectId === project.projectId)
        );
        this.tableVisible = this.commonProjects.map(() => true);
        this.splitIssuesByProject(this.userIssues, this.commonProjects);
      });
    }
  }

  onSubmit(): void {
    // Update user data
  }
}