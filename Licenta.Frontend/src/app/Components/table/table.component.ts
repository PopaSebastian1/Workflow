import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { Issue } from '../../Models/Issue';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.scss']
})
export class TableComponent implements OnInit {
  displayedColumns: string[] = [
    'number',
    'title',
    'status',
    'type',
    'createdOn',
    'reporter',
    'assignee',
  ];
  private _issues: Issue[] = [];

  @Input() 
  set issues(issues: Issue[]) {
    this._issues = issues;
    this.dataSource.data = issues;
  }
  
  get issues(): Issue[] {
    return this._issues;
  }
  dataSource = new MatTableDataSource<Issue>(this.issues);
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngOnInit() {
    console.log(this.issues);
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }
}