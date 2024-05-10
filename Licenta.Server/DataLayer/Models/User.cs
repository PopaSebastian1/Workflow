using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Licenta.Server.DataLayer.Models
{
    public class User : IdentityUser<Guid>
    {
        public Role Role { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [JsonIgnore]
        [NotMapped]
        public List<Skill>? Skills { get; set; }
        [JsonIgnore]
        public List<Project> Projects { get; set; }
        [JsonIgnore]
        public List<Issue> Issues { get; set; }
        [JsonIgnore]
        public List<Project> AdministratorProjects { get; set; }
        public string? ProfilePicture { get; set; }

        //create fullName property for the user that comes from firstName and lastName
        public string? FullName => $"{FirstName} {LastName}";

   
        
    }
}
