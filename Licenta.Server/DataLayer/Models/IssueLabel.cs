using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Licenta.Server.DataLayer.Models
{
    public class IssueLabel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public Guid ProjectId { get; set; }
    }
}
