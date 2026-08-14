namespace SchoolEquipmentApi.DTOs
{
    public class BuildingDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ClassroomsCount { get; set; }
    }

    public class CreateBuildingDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
    }

    public class UpdateBuildingDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
    }
}