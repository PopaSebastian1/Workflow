namespace Licenta.Server.DataLayer.Dto
{
    public class CreateProjectDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid OwnerId { get; set; }
    }
}
