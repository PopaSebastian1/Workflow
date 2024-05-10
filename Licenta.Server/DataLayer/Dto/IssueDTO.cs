using Licenta.Server.DataLayer.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Licenta.Server.DataLayer.Dto
{
    public class IssueDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DueDate { get; set; }
        public float EstimatedTime { get; set; }
        public float LoggedTime { get; set; }

        // Chei externe
        [ForeignKey("Project")]
        public Guid ProjectId { get; set; }

        [ForeignKey("Sprint")]
        public Guid SprintId { get; set; }

        [ForeignKey("Assignee")]
        public Guid AssigneeId { get; set; }

        [ForeignKey("Reporter")]
        public Guid ReporterId { get; set; }

        [ForeignKey("IssueStatus")]
        public int IssueStatusId { get; set; }

        [ForeignKey("IssueType")]
        public int IssueTypeId { get; set; }

        [ForeignKey("ParentIssue")]
        public Guid? ParentIssueId { get; set; }
        // Navigation properties
        public List<Comment> Comments { get; set; }
        public Project Project { get; set; }
        public Sprint Sprint { get; set; }
        public IssueStatus IssueStatus { get; set; }
        public IssueType IssueType { get; set; }
        public User Assignee { get; set; }
        public User Reporter { get; set; }

        // Proprietăți pentru relația de ierarhie
        public Issue ParentIssue { get; set; }

        // Lista de probleme copil
        public List<Issue> ChildIssues { get; set; }
    }
}
