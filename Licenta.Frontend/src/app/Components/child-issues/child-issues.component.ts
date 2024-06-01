import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { IssueService } from '../../Services/issue-service';
import { Issue } from '../../Models/Issue';

@Component({
  selector: 'app-child-issues',
  templateUrl: './child-issues.component.html',
  styleUrls: ['./child-issues.component.scss']
})
export class ChildIssuesComponent implements OnInit{
  @Input() currentIssue?: Issue;
  displayedColumns: string[] = ['title', 'issueStatus', 'issueType', 'assignee', 'actions'];
  dataSource: Issue[] = [];
  @Output() removeIssueEvent = new EventEmitter();
  constructor(issueService: IssueService) {
    
  }
  ngOnInit(): void {
    console.log(this.currentIssue?.sprint);
    this.dataSource = this.currentIssue?.childIssues || [];
    console.log(this.dataSource[0].sprint);
  }
  convertIssueStatus(nr:number)
  {
    if(nr==1)
      {
      return "To Do"
    }
    if(nr==2)
      {
        return "In Progress"
      }
      if(nr==3)
    {
      return "Done"
    }
    else
    {
      return "Unknown"
    }
  }
  convertIssueType(nr:number)
  {
    if(nr==2)
    {
      return "Story"
    }
    if(nr==6)
    {
      return "Bug"
    }
    else
    {
      return "Task"
    }
  }
  removeIssue(issueId: string) {
    this.removeIssueEvent.emit(issueId);
  }
}