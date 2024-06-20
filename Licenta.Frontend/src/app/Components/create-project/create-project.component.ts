// import { Component } from '@angular/core';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AddProjectDto } from '../../Models/DTO/AddProjectDto';
import { User } from '../../Models/User';
import { DataService } from '../../Services/data-service';
import { ProjectService } from '../../Services/project-service';
@Component({
  selector: 'app-create-project',
  templateUrl: './create-project.component.html',
  styleUrl: './create-project.component.scss',
})
export class CreateProjectComponent {
  user: User | null = null;
  addProject: AddProjectDto = new AddProjectDto();
  @Input() title: string = 'Create project';
  @Output() closeModalEvent = new EventEmitter<void>();
  projectName: string = '';
  projectDescription: string = '';
  projectDueDate: Date = new Date();
  //projectStartDate should be today
  projectStartDate: Date = new Date();

  constructor(
    private dataService: DataService,
    private projectService: ProjectService
  ) {
    this.user = JSON.parse(localStorage.getItem('user') || '{}');
    this.addProject.description = this.projectDescription;
    // Fix: check for undefined before assigning to name
    if (this.projectDescription) {
      this.addProject.name = this.projectDescription;
    }
    this.addProject.endDate = this.projectDueDate;
    this.addProject.startDate = this.projectStartDate;
  }

  createProject() {
    this.addProject.name = this.projectName;
    this.addProject.description = this.projectDescription;
    let dueDate = new Date(this.projectDueDate);
    let startDate = new Date(this.projectStartDate);
    
    dueDate.setUTCHours(dueDate.getHours());
    startDate.setUTCHours(startDate.getHours());

    this.addProject.endDate = this.projectDueDate;
    this.addProject.startDate = this.projectStartDate;
    this.addProject.ownerId = String(this.user?.id);

    if (this.addProject.name && this.addProject.description) {
      this.projectService.addProject(this.addProject, this.user?.id).subscribe({
        next: (project) => {
          // project created successfully
          console.log(project);
          this.closeModal();
        },
        error: (error) => {
          console.error(error);
          // handle error
        },
      });
    }
  }

  closeModal() {
    this.closeModalEvent.emit();
  }

  onSubmit() {
    this.createProject();
  }
}
