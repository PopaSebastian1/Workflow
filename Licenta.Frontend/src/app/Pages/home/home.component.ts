import { Component, OnInit } from '@angular/core';
import { ProjectService } from '../../Services/project-service'; // import ProjectService
import { User } from '../../Models/User';
import { EMPTY, Observable } from 'rxjs';
import { Project } from '../../Models/Project';
import { CreateProjectComponent } from '../../Components/create-project/create-project.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  userProjects: Observable<Project[]> = EMPTY; // declare userProjects as a variable of type Observable<Project[]>
  user: User | null = null; // declare user as a variable of type User or null
  showCreateProject: boolean = false;
  showJoinProject: boolean = false;
  selectedProject: Project | null = null;
  constructor(private projectService: ProjectService, private router: Router) {} // inject ProjectService
  setSelectedProject(project: Project) {
    this.selectedProject = project;
  }

  goToProject(proiect: Project) {
    this.projectService.setSelectedProject(proiect);
    //go to the issues component
    this.router.navigate(['issues'], {
      queryParams: { projectId: this.selectedProject?.projectId },
    });
  }

  getFormattedDate(date: Date): string {
    return `${date.getMonth() + 1}/${date.getFullYear()}`;
  }
  toggleJoinProject() {
    this.showJoinProject = !this.showJoinProject;
  }

  toggleCreateProject() {
    this.showCreateProject = !this.showCreateProject;
  }

  closeCreateProject() {
    this.showCreateProject = false;
    if (this.user && this.user.email) {
      this.userProjects = this.projectService.getUserProjectsFromApi(
        this.user.email
      );
    }
  }
  closejoinProject() {
    this.showJoinProject = false;
    if (this.user && this.user.email) {
      this.userProjects = this.projectService.getUserProjectsFromApi(
        this.user.email
      );
    }
  }
  removeProject() {
    if (
      this.selectedProject &&
      this.selectedProject.ownerId === this.user?.id.toString()
    ) {
      this.projectService
        .deleteProject(this.selectedProject.projectId)
        .subscribe(() => {
          // Update the userProjects list after removing the project
          if (this.user && this.user.email) {
            this.userProjects = this.projectService.getUserProjectsFromApi(
              this.user.email
            );
          }
        });
    } else if (
      this.selectedProject &&
      this.selectedProject.ownerId !== this.user?.id.toString()
    ) {
      {
        this.projectService
          .leaveProject(this.selectedProject.projectId)
          .subscribe(() => {
            
            if (this.user && this.user.email) {
              this.userProjects = this.projectService.getUserProjectsFromApi(
                this.user.email
              );
            }
          });
      }
    }
  }

  ngOnInit(): void {
    let userItem = localStorage.getItem('user');
    this.user = userItem ? JSON.parse(userItem) : null; // add null check and provide default value

    if (this.user && this.user.email) {
      this.userProjects = this.projectService.getUserProjectsFromApi(
        this.user.email
      );
    }
    this.userProjects.subscribe((projects) => {
      console.log(projects);
    });
  }
}
