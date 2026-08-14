namespace SchoolEquipmentApi.DTOs
{
    public class EquipmentTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int EquipmentCount { get; set; }
    }

    public class CreateEquipmentTypeDto
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }

    public class UpdateEquipmentTypeDto
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }
}