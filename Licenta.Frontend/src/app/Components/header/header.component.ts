import { Component } from '@angular/core';
import { User } from '../../Models/User';
import { DataService } from '../../Services/data-service';
import { Router } from '@angular/router';
import { OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss',
})
export class HeaderComponent implements OnInit {
  user: User | null = null;
  userMenu: boolean = false;
  constructor(private dataService: DataService, private router: Router) { }
  ngOnInit() {
    this.user = this.dataService.getUserData();
    console.log(this.user);
  }
  toggleUserMenu() {
    this.userMenu = !this.userMenu;
  }
  logOut() {
    this.user = null;
    this.dataService.logOut();
    this.router.navigate(['/login']);
  }
}
