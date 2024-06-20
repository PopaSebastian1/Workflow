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
  showMenu = false;
  images = [
    {name: 'Image 1', url: 'https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcTjo-4GdQ00VYxRdpbKtXs0DxjjYi_4iY8w82SxAeBFOMHnujQM'},
    {name: 'Image 2', url: 'https://static.vecteezy.com/system/resources/previews/005/036/044/non_2x/trendy-teacher-concepts-vector.jpg'},
    {name: 'Image 3', url: 'https://cdn3.iconfinder.com/data/icons/business-avatar-1/512/11_avatar-512.png'},
    {name: 'Image 4', url: 'https://cdn.icon-icons.com/icons2/2643/PNG/512/male_boy_person_people_avatar_icon_159358.png'},
    {name: 'Image 5', url: 'https://banner2.cleanpng.com/20210721/osx/transparent-jedi-avatar-60f79d68da49c0.1144258416268404248941.jpg'},
    {name: 'Image 6', url: 'https://w7.pngwing.com/pngs/129/292/png-transparent-female-avatar-girl-face-woman-user-flat-classy-users-icon.png'},
  ];
toggleMenu() {
  this.showMenu = !this.showMenu;
}
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
  selectProfilePhoto(imageName: string) {
    if (this.userProfile) {
      this.userProfile.profilePicture = imageName;
      this.userService.updateProfilePhoto(this.userId, imageName).subscribe(() => {
        
      });
      this.userService.setUserData(this.userProfile);
    }
    window.location.reload()
  }
  splitIssuesByProject(issues: Issue[], projects: Project[]) {
    projects.forEach((project, index) => {
      const projectIssues = issues.filter((issue) => issue.projectId === project.projectId);
      this.projectIssuesMap[project.projectId] = projectIssues;
      this.projectIssueDataSources[index] = new MatTableDataSource(projectIssues);
      this.projectIssueDataSources[index].paginator = this.paginators.toArray()[index];

    });
  }

  ngAfterViewChecked() {
    this.paginators.toArray().forEach((paginator, index) => {
      if (this.projectIssueDataSources[index].paginator != paginator) {
        this.projectIssueDataSources[index].paginator = this.paginators.toArray()[index];
      }
    });
  
  }
  toggleTable(index: number) {
    this.tableVisible[index] = !this.tableVisible[index];
    // Dacă tabelul este acum vizibil, actualizăm paginatorul pentru sursa de date corespunzătoare.

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