using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Licenta.Server.DataLayer.Models
{
    public class Project
    {
        public Guid ProjectId { get; set; }
 
        public string Name { get; set; }
        public string Description { get; set; }
        public string Key { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [ForeignKey("OwnerId")]
        public Guid OwnerId { get; set; }
        public User Owner { get; set; }
        public List<Sprint> Sprints { get; set; }
        public List<Issue> Issues { get; set; }
        public List<User> Members { get; set; }
        public List<IssueStatus> IssueStatuses { get; set; }
        public List<User> Administrators { get; set; }
        public Project()
        {
            Sprints = new List<Sprint>();
            Members = new List<User>();
        }

    }
}
