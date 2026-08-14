namespace SchoolEquipmentApi.Models
{
    public class EquipmentSpecification
    {
        public int Id { get; set; }
        public int EquipmentId { get; set; }
        public int? CategoryId { get; set; }
        public string Value { get; set; } = string.Empty;
        public string? CustomName { get; set; }

        public Equipment Equipment { get; set; } = null!;
        public SpecificationCategory? Category { get; set; }
    }
}