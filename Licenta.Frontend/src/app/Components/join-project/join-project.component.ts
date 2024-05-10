import { Component, EventEmitter, Output } from '@angular/core';
import { ProjectService } from '../../Services/project-service';
@Component({
  selector: 'app-join-project',
  templateUrl: './join-project.component.html',
  styleUrl: './join-project.component.scss',
})
export class JoinProjectComponent {
  @Output() closeModalEvent = new EventEmitter<void>();
  projectCode: any;

  constructor(private projectService: ProjectService) {}

  onSubmit() {
    this.projectService.joinProject(this.projectCode).subscribe(
        response => {
            // Handle the response here if needed
            this.closeModalEvent.emit();
        },
        error => {
            // Handle any errors here
        }
    );
}
  onDialogClose() {
    this.closeModalEvent.emit();
  }
}
