import { Component, Input, OnInit } from '@angular/core';
import { IssueService } from '../../Services/issue-service';
import { Issue } from '../../Models/Issue';

@Component({
  selector: 'app-child-issues',
  templateUrl: './child-issues.component.html',
  styleUrls: ['./child-issues.component.scss']
})
export class ChildIssuesComponent implements OnInit{
  @Input() currentIssue?: Issue;
  displayedColumns: string[] = ['title', 'issueStatus', 'issueType', 'assignee'];
  dataSource: Issue[] = [
  ];
  constructor() { }
  ngOnInit(): void {
    console.log(this.currentIssue?.sprint);
    this.dataSource = this.currentIssue?.childIssues || [];
    console.log(this.dataSource[0].sprint);
  }
}