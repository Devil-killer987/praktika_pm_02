using SchoolEquipmentApi.Models;

namespace SchoolEquipmentApi.DTOs
{
    public class EquipmentDto
    {
        public int Id { get; set; }
        public int ClassroomId { get; set; }
        public string ClassroomNumber { get; set; } = string.Empty;
        public int EquipmentTypeId { get; set; }
        public string EquipmentTypeName { get; set; } = string.Empty;
        public string InventoryNumber { get; set; } = string.Empty;
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public string? SerialNumber { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? WarrantyEnd { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public List<SpecificationDto> Specifications { get; set; } = new();
    }

    public class CreateEquipmentDto
    {
        public int ClassroomId { get; set; }
        public int EquipmentTypeId { get; set; }
        public string InventoryNumber { get; set; } = string.Empty;
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public string? SerialNumber { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? WarrantyEnd { get; set; }
        public string Status { get; set; } = "Working";
        public string? Notes { get; set; }
        public List<CreateSpecificationDto> Specifications { get; set; } = new();
    }

    public class UpdateEquipmentDto
    {
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public string? SerialNumber { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? WarrantyEnd { get; set; }
        public string Status { get; set; } = "Working";
        public string? Notes { get; set; }
        public List<CreateSpecificationDto> Specifications { get; set; } = new();
    }

    public class EquipmentFilterDto
    {
        public int? ClassroomId { get; set; }
        public int? EquipmentTypeId { get; set; }
        public string? Status { get; set; }
        public string? SearchTerm { get; set; }
    }
}