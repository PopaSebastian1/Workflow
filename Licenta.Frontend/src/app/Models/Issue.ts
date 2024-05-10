import { IssueStatus } from "./IssueStatus";
import { IssueType } from "./IssueType";
import { Project } from "./Project";
import { Sprint } from "./Sprint";
import { User } from "./User";

export interface Issue {
    id: string;
    title: string;
    description: string;
    createdAt: Date;
    dueDate: Date;
    estimatedTime: number;
    loggedTime: number;
    projectId: string;
    sprintId: string;
    assigneeId: string;
    reporterId: string;
    issueStatusId: number;
    issueTypeId: number;
    parentIssueId: string | null;
    comments: Comment[];
    project: Project;
    sprint: Sprint;
    issueStatus: IssueStatus;
    issueType: IssueType;
    assignee: User;
    reporter: User;
    parentIssue: Issue;
    childIssues: Issue[];

}