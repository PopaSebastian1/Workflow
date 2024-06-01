import { Issue } from "./Issue";
import { Project } from "./Project";
import { SprintStatus } from "./SprintStatus";

export interface Sprint {
    id: string;
    name: string;
    startDate: Date;
    endDate: Date;
    projectId: string;
    project: Project;
    issues: Issue[];
    status:SprintStatus
}