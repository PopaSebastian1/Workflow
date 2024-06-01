import { Component, OnInit } from '@angular/core';
import { User } from '../../Models/User';
import { ProjectService } from '../../Services/project-service';
import { DataService } from '../../Services/data-service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-team',
  templateUrl: './team.component.html',
  styleUrl: './team.component.scss',
})
export class TeamComponent implements OnInit {
  admins: User[] = [];
  users: User[] = [];
  user:User | null= null;
  constructor(private projectService: ProjectService, private dataService: DataService, private router: Router) { }
  viewProfile(userId: string) {
    this.router.navigate(['/profile', userId]);
  }
  deleteUser(email: string) {
    this.projectService.removeMemberFromProject(email).subscribe(() => {
      this.users = this.users.filter(user => user.email!== email);
    });
  }
  isAdmin(user: User): boolean {
    return this.admins.some(admin => admin.id === user.id);
  }
  toggleAdminStatus(user: User) {
    if (this.isAdmin(user)) {
      this.admins = this.admins.filter(admin => admin.id !== user.id);
    } else {
      this.projectService.addAdminstratorToProject(user.email).subscribe(() => {
        this.projectService.getAllMembers().subscribe((users) => {
          this.users = users;
        });
        this.projectService.getAllAdministratorsForProject().subscribe((admins) => {
          this.admins = admins;
        });
      });
    }
  }
  currentUserIsAdmin(): boolean {
    if(this.user){
      return this.admins.some(admin => admin.id === this.user?.id);
    }
    return false;
  }
  
  ngOnInit() {
    this.projectService.getAllMembers().subscribe((users) => {
      this.users = users;
    });
    this.projectService.getAllAdministratorsForProject().subscribe((admins) => {
      this.admins = admins;
    });
  this.user = this.dataService.getUserData();
  console.log(this.user);
  }
}
