import { Component } from '@angular/core';
import { User } from '../../Models/User';
import { DataService } from '../../Services/data-service';
import { Role } from '../../Models/Role';
import { SkillLevel } from '../../Models/SkillLevel';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent {
  user:User | null = null;
  
  constructor(private userService: DataService) {}

  getUserData() {
    this.user = this.userService.getUserData();
  }

  getRoleString(role:Role | undefined)
  {
    if(role==Role.Admin)
    {
      return "Admin";
    }
    else if(role==Role.User)
    {
      return "User";
    }
    else
    {
      return "Unknown";
    }
  }

  getSkillLevel(level:number):string
  {
    return SkillLevel[level];
  }

  ngOnInit() {
    this.getUserData();
    console.log(this.user);
  }

}
