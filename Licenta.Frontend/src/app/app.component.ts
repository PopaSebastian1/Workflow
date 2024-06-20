import { Component } from '@angular/core';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Workflow';
  icon= 'assets/W.png';
  isLoginPage:boolean = false;
  isRegisterParge:boolean = false;
  constructor(private router: Router, private activatedRoute: ActivatedRoute) {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      this.isLoginPage = this.activatedRoute.snapshot.firstChild?.url[0].path === 'login';
      this.isRegisterParge = this.activatedRoute.snapshot.firstChild?.url[0].path ==='register';
    });
  }
}