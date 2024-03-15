namespace Licenta.Server.DataLayer.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }

    }
}
