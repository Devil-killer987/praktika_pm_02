namespace SchoolEquipmentApi.DTOs
{
    public class ClassroomDto
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public string BuildingName { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public string? Floor { get; set; }
        public string? Description { get; set; }
        public int EquipmentCount { get; set; }
    }

    public class CreateClassroomDto
    {
        public int BuildingId { get; set; }
        public string Number { get; set; } = string.Empty;
        public string? Floor { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateClassroomDto
    {
        public string Number { get; set; } = string.Empty;
        public string? Floor { get; set; }
        public string? Description { get; set; }
    }
}