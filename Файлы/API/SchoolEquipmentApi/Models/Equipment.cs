namespace SchoolEquipmentApi.Models
{
    public class Equipment
    {
        public int Id { get; set; }
        public int ClassroomId { get; set; }
        public int EquipmentTypeId { get; set; }
        public string InventoryNumber { get; set; } = string.Empty;
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public string? SerialNumber { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? WarrantyEnd { get; set; }
        public string Status { get; set; } = "Working"; // Working, Repair, Decommissioned
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public Classroom Classroom { get; set; } = null!;
        public EquipmentType EquipmentType { get; set; } = null!;
        public ICollection<EquipmentSpecification> Specifications { get; set; } = new List<EquipmentSpecification>();
    }
}