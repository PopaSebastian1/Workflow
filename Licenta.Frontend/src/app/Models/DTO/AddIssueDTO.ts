
export class AddIssueDTO
{
    Id: string
    Title: string;
    Description: string;
    CreatedAt: Date;
    DueDate: Date;
    EstimatedTime: number;
    LoggedTime: number;
    ProjectId: string;
    SprintId: string;
    AssigneEmail: string;
    ReporterEmail: string;
    IssueStatus: string;
    IssueType: string; 
    ParentIssueId: string;
    constructor()
    {
        this.Id = '';
        this.Title = '';
        this.Description = '';
        this.CreatedAt = new Date();
        this.DueDate = new Date();
        this.EstimatedTime = 0;
        this.LoggedTime = 0;
        this.ProjectId = '';
        this.SprintId = '';
        this.AssigneEmail = '';
        this.ReporterEmail = '';
        this.IssueStatus = '';
        this.IssueType = '';
        this.ParentIssueId = '';
    }
}
