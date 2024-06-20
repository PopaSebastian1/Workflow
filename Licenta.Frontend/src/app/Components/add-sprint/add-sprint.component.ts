import { Component, EventEmitter, Output } from '@angular/core';
import { SprintService } from '../../Services/sprint-service';
import { Sprint } from '../../Models/Sprint';
import { ProjectService } from '../../Services/project-service';
import { SprintStatus } from '../../Models/SprintStatus';
@Component({
  selector: 'app-add-sprint',
  templateUrl: './add-sprint.component.html',
  styleUrls: ['./add-sprint.component.scss'],
})
export class AddSprintComponent {
  @Output() closeModalEvent = new EventEmitter();
  @Output() submitEvent = new EventEmitter();
  name = '';
  startDate = '';
  endDate = '';

  constructor(private sprintService: SprintService, private projectService: ProjectService) {

   }

  close() {
    this.closeModalEvent.emit();
  }

  onSubmit() {
    const sprint: Sprint = {
      id: '',
      name: this.name,
      startDate: new Date(this.startDate),
      endDate: new Date(this.endDate),
      projectId: this.projectService.selectedProject?.projectId || '',
      project: this.projectService.selectedProject!,
      issues: [],
      status: SprintStatus.Planned,
    };

    this.sprintService.addSprint(sprint).subscribe({
      next: (sprint) => {
        console.log(sprint);
      },
      error: (err) => {
        console.log(err);
      },
    });

    this.submitEvent.emit();
    this.close();
  }
}