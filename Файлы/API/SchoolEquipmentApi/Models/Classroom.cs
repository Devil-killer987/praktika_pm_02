namespace SchoolEquipmentApi.Models
{
    public class Classroom
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public string Number { get; set; } = string.Empty;
        public string? Floor { get; set; }
        public string? Description { get; set; }

        public Building Building { get; set; } = null!;
        public ICollection<Equipment> Equipment { get; set; } = new List<Equipment>();
    }
}