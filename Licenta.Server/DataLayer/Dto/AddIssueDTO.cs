using Licenta.Server.DataLayer.Enum;

namespace Licenta.Server.DataLayer.Dto
{
    public class AddIssueDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DueDate { get; set; }
        public float EstimatedTime { get; set; }
        public float LoggedTime { get; set; }
        public Guid ProjectId { get; set; }
        public Guid SprintId { get; set; }
        public string AssigneEmail { get; set; }
        public string ReporterEmail { get; set; }
        public string IssueStatus { get; set; }
        public string IssueType { get; set; }
        public Guid ParentIssueId { get; set; }
        public int? LabelId { get; set; }
        public PriorityLevel Priority { get; set; }

    }
}
