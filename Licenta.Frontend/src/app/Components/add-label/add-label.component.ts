import { Component, EventEmitter, Output } from '@angular/core';
import { IssueLabel } from '../../Models/IssueLabel';
import { ProjectService } from '../../Services/project-service';

@Component({
  selector: 'app-add-label',
  templateUrl: './add-label.component.html',
  styleUrls: ['./add-label.component.scss'],
})
export class AddLabelComponent {
  @Output() closeModalEvent = new EventEmitter();
  @Output() submitEvent = new EventEmitter<IssueLabel>();

  color: string = '';
  name: string = '';

  constructor(private projectService: ProjectService) {}

  onSubmit() {
    let label: IssueLabel = { color: this.color,
       name: this.name,
       id:0,
       projectId: this.projectService.selectedProject?.projectId || ''
     };

    this.projectService.addLabelToProject(label).subscribe(
      () => {
        this.submitEvent.emit(label);
        this.close();
      },
      error => {
        console.error('Error adding label: ', error);
      }
    );
  }

  close() {
    this.closeModalEvent.emit();
  }
}