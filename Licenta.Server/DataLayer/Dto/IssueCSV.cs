namespace Licenta.Server.DataLayer.Dto;

public class IssueCSV
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string CreatedAt { get; set; }
    public string DueDate { get; set; }
    public string IssueType { get; set; }
    public string IssueStatus { get; set; }
    public float EstimatedTime { get; set; }
    public float LoggedTime { get; set; }
    public string ProjectName { get; set; }
    public string SprintName { get; set; }
    public string AssigneEmail { get; set; }
    public string ReporterEmail { get; set; }
}
