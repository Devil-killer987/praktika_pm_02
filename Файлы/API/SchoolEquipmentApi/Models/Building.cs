namespace SchoolEquipmentApi.Models
{
    public class Building
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public ICollection<Classroom> Classrooms { get; set; } = new List<Classroom>();
    }
}