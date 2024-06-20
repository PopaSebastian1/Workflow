import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { IssueService } from '../../Services/issue-service';
import { Issue } from '../../Models/Issue';

@Component({
  selector: 'app-child-issues',
  templateUrl: './child-issues.component.html',
  styleUrls: ['./child-issues.component.scss'],
})
export class ChildIssuesComponent implements OnInit {
  @Input() currentIssue?: Issue;
  displayedColumns: string[] = [
    'title',
    'issueStatus',
    'issueType',
    'assignee',
    'actions',
  ];
  @Output() removeIssueEvent = new EventEmitter();
  dataSource: Issue[] = [];
  constructor(private issueService: IssueService) {}
  ngOnInit(): void {
    console.log(this.currentIssue?.sprint);
    this.issueService.getAllChildIssues(this.currentIssue!.id).subscribe(
      (issues) => {
        this.dataSource = issues;
        console.log(this.dataSource);
      },
      (error) => {
        console.log(error);
      }
    );
    console.log(this.dataSource[0].assignee);
  }
  convertIssueStatus(nr: number) {
    if (nr == 1) {
      return 'To Do';
    }
    if (nr == 2) {
      return 'In Progress';
    }
    if (nr == 3) {
      return 'Done';
    } else {
      return 'Unknown';
    }
  }
  convertIssueType(nr: number) {
    if (nr == 2) {
      return 'Story';
    }
    if (nr == 6) {
      return 'Bug';
    } else {
      return 'Task';
    }
  }
  removeIssue(issueId: string) {
    this.removeIssueEvent.emit(issueId);
  }
}
