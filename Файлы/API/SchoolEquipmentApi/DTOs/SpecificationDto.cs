namespace SchoolEquipmentApi.DTOs
{
    public class SpecificationDto
    {
        public int Id { get; set; }
        public int EquipmentId { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryDisplayName { get; set; }
        public string Value { get; set; } = string.Empty;
        public string? CustomName { get; set; }
    }

    public class CreateSpecificationDto
    {
        public int? CategoryId { get; set; }
        public string Value { get; set; } = string.Empty;
        public string? CustomName { get; set; }
    }

    public class SpecificationCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string? Unit { get; set; }
        public int EquipmentTypeId { get; set; }
        public string EquipmentTypeName { get; set; } = string.Empty;
    }

    public class CreateSpecificationCategoryDto
    {
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string? Unit { get; set; }
        public int EquipmentTypeId { get; set; }
    }
}