import { Issue } from "./Issue";
import { Project } from "./Project";

export interface Sprint {
    id: string;
    name: string;
    startDate: Date;
    endDate: Date;
    projectId: string;
    project: Project;
    issues: Issue[];
}