import { Issue } from "./Issue";
import { IssueLabel } from "./IssueLabel";
import { Sprint } from "./Sprint";
import { User } from "./User";

export interface Project {
    projectId: string;
    name: string;
    description: string;
    startDate: Date;
    endDate: Date;
    ownerId: string;
    owner: User;
    sprints: Sprint[];
    issues: Issue[];
    members: User[];
    issueLabels: IssueLabel[];
    key: string;
}