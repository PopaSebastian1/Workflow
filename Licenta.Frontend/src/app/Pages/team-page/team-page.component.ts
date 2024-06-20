import { Component, OnInit } from '@angular/core';
import { Project } from '../../Models/Project';
import { ProjectService } from '../../Services/project-service';
import { User } from '../../Models/User';
import { DataService } from '../../Services/data-service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-team-page',
  templateUrl: './team-page.component.html',
  styleUrls: ['./team-page.component.scss']
})
export class TeamPageComponent implements OnInit {
  projects: Project[] = [];
  user: User | null = null;
  isAdmin: boolean = false;
  copySuccess: boolean = false;

  constructor(private projectService: ProjectService, private dataService: DataService) { }

  ngOnInit(): void {
      this.user=this.dataService.getUserData();
      this.projectService.getAllAdministratorsForProject().subscribe((admins) => {
        this.isAdmin = admins.some(admin => admin.id === this.user?.id);
      });
  }
  copyProjectKey(): void {
    let project = this.projectService.getSelectedProject();
    if (project) {
      navigator.clipboard.writeText(project.key).then(() => {
        this.copySuccess = true;
        setTimeout(() => {
          this.copySuccess = false;
        }, 1000); // 1000 milliseconds = 1 second
      });
    }
  }
  
}