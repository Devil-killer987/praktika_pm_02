namespace SchoolEquipmentApi.Models
{
    public class SpecificationCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string? Unit { get; set; }
        public int EquipmentTypeId { get; set; }

        public EquipmentType EquipmentType { get; set; } = null!;
        public ICollection<EquipmentSpecification> Specifications { get; set; } = new List<EquipmentSpecification>();
    }
}