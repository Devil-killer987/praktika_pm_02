namespace SchoolEquipmentApi.Models
{
    public class EquipmentType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // PC, Printer, Projector, Network, Other

        public ICollection<Equipment> Equipment { get; set; } = new List<Equipment>();
        public ICollection<SpecificationCategory> SpecificationCategories { get; set; } = new List<SpecificationCategory>();
    }
}