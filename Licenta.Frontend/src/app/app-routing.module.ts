import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './Pages/login/login.component'; // adjust this import to your file structure
import { RegisterComponent } from './Pages/register/register.component'; // adjust this import to your file structure
import { HomeComponent } from './Pages/home/home.component';
import { ProfileComponent } from './Pages/profile/profile.component';
import { SidenavComponent } from './sidenav/sidenav.component';
import { IssueListComponent } from './Pages/issue-list/issue-list.component';
import { IssueDetailsComponent } from './Components/issue-details/issue-details.component';
import { TeamPageComponent } from './Pages/team-page/team-page.component';
import { SprintComponent } from './Components/sprint/sprint.component';
import { SprintPageComponent } from './Pages/sprint-page/sprint-page.component';
import { DashboardComponent } from './Pages/dashboard/dashboard.component';
const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' }, // redirect to `login-component`
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'home', component: HomeComponent },
  { path: 'profile/:id', component: ProfileComponent },
  { path: 'sidenav', component: SidenavComponent },
  { path: 'issues', component: IssueListComponent },
  { path: 'issuedetails/:id', component: IssueDetailsComponent },
  { path: 'team', component: TeamPageComponent },
  { path: 'sprint', component: SprintPageComponent },
  { path: 'dashboard', component: DashboardComponent },
  // other routes...
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
