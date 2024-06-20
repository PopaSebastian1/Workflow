import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './Pages/login/login.component';
import { RegisterComponent } from './Pages/register/register.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { HomeComponent } from './Pages/home/home.component';
import { ProfileComponent } from './Pages/profile/profile.component';
import { SidenavComponent } from './sidenav/sidenav.component';
import { MatListModule } from '@angular/material/list';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { BodyComponent } from './body/body.component';
import { HeaderComponent } from './Components/header/header.component';
import { CreateProjectComponent } from './Components/create-project/create-project.component';
import { IssueListComponent } from './Pages/issue-list/issue-list.component';
import { IssueDetailsComponent } from './Components/issue-details/issue-details.component';
import { IssueCardComponent } from './Components/issue-card/issue-card.component';
import { MatTableModule } from '@angular/material/table';
import { AddIssueComponent } from './Components/add-issue/add-issue.component';
import { MatFormField, MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatDialogModule } from '@angular/material/dialog';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { DatePipe } from '@angular/common';
import { JoinProjectComponent } from './Components/join-project/join-project.component';
import { TeamComponent } from './Components/team/team.component';
import { TeamPageComponent } from './Pages/team-page/team-page.component';
import { MatMenuModule } from '@angular/material/menu';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { ChildIssuesComponent } from './Components/child-issues/child-issues.component';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { SprintComponent } from './Components/sprint/sprint.component';
import { SprintPageComponent } from './Pages/sprint-page/sprint-page.component';
import { StatisticsComponent } from './Components/statistics/statistics.component';
import { DashboardComponent } from './Pages/dashboard/dashboard.component';
import { TableComponent } from './Components/table/table.component';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { AddSprintComponent } from './Components/add-sprint/add-sprint.component';
import { AddLabelComponent } from './Components/add-label/add-label.component';
import { MatSortModule } from '@angular/material/sort';
import { ConfirmDeleteDialogComponent } from './Components/confirm-delete-dialog/confirm-delete-dialog.component';
@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    HomeComponent,
    ProfileComponent,
    SidenavComponent,
    BodyComponent,
    HeaderComponent,
    CreateProjectComponent,
    IssueListComponent,
    IssueDetailsComponent,
    IssueCardComponent,
    AddIssueComponent,
    JoinProjectComponent,
    TeamComponent,
    TeamPageComponent,
    ChildIssuesComponent,
    SprintComponent,
    SprintPageComponent,
    StatisticsComponent,
    DashboardComponent,
    TableComponent,
    AddSprintComponent,
    AddLabelComponent,
    ConfirmDeleteDialogComponent,
  ],
  imports: [
    BrowserModule,
    ReactiveFormsModule,
    HttpClientModule,
    AppRoutingModule,
    MatListModule,
    FormsModule,
    MatTableModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatOptionModule,
    MatProgressBarModule,
    MatPaginatorModule,
    MatDialogModule,
    MatMenuModule,
    MatIconModule,
    MatCheckboxModule,
    MatButtonModule,
    BrowserAnimationsModule,
    MatSortModule
  ],
  providers: [
    [DatePipe],
    provideAnimationsAsync('noop'),
    provideAnimationsAsync(),
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
