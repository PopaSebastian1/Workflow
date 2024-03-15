namespace Licenta.Server.DataLayer.Dto
{
    public class AddIssueDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DueDate { get; set; }
        public float EstimatedTime { get; set; }
        public float LoggedTime { get; set; }
        public Guid ProjectId { get; set; }
        public Guid SprintId { get; set; }
        public Guid AssigneeId { get; set; }
        public Guid ReporterId { get; set; }
        public int IssueStatusId { get; set; }
        public int IssueTypeId { get; set; }

    }
}
