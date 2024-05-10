using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using JsonIgnoreAttribute = System.Text.Json.Serialization.JsonIgnoreAttribute;

namespace Licenta.Server.DataLayer.Models
{
    public class Issue
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
        [JsonIgnore]
        public List<Comment> Comments { get; set; }
        [JsonIgnore]
        public Project Project { get; set; }
        public Sprint Sprint { get; set; }
        public IssueStatus IssueStatus { get; set; }
        public IssueType IssueType { get; set; }
        public User Assignee { get; set; }
        public User Reporter { get; set; }
        // Proprietăți pentru relația de ierarhie
        [JsonIgnore]
        public Issue ParentIssue { get; set; }
        // Lista de probleme copil
        //[JsonIgnore]
        public List<Issue> ChildIssues { get; set; }

        // Cheia externă pentru relația de unu-la-multe cu problema părinte
    }
}