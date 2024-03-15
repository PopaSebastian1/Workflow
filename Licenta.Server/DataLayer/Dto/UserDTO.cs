using Licenta.Server.DataLayer.Models;

namespace Licenta.Server.DataLayer.Dto
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public Role? Role { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ProfilePicture { get; set; }
        public string ? FullName => $"{FirstName} {LastName}";
        public List<Skill>? Skills { get; set; }
        public List<Project>? Projects { get; set; }
    }
}
