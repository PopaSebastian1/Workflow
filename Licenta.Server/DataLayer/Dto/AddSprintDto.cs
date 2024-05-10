namespace Licenta.Server.DataLayer.Dto
{
    public class AddSprintDto
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid ProjectId { get; set; }
    }
}
