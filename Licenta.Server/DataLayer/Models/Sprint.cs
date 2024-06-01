using Licenta.Server.DataLayer.Enum;
using Microsoft.Build.Evaluation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Licenta.Server.DataLayer.Models
{
    public class Sprint
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public List<Issue> Issues { get; set; }

        public SprintStatus Status { get; set; }
    }
}
