using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolEquipmentApi.Data;
using SchoolEquipmentApi.DTOs;
using SchoolEquipmentApi.Models;

namespace SchoolEquipmentApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EquipmentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EquipmentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] EquipmentFilterDto filter)
        {
            var query = _context.Equipment
                .Include(e => e.Classroom)
                .Include(e => e.EquipmentType)
                .Include(e => e.Specifications)
                    .ThenInclude(s => s.Category)
                .AsQueryable();

            if (filter.ClassroomId.HasValue)
                query = query.Where(e => e.ClassroomId == filter.ClassroomId.Value);

            if (filter.EquipmentTypeId.HasValue)
                query = query.Where(e => e.EquipmentTypeId == filter.EquipmentTypeId.Value);

            if (!string.IsNullOrEmpty(filter.Status))
                query = query.Where(e => e.Status == filter.Status);

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                var search = filter.SearchTerm.ToLower();
                query = query.Where(e => 
                    e.InventoryNumber.ToLower().Contains(search) ||
                    (e.Model != null && e.Model.ToLower().Contains(search)) ||
                    (e.SerialNumber != null && e.SerialNumber.ToLower().Contains(search)) ||
                    (e.Manufacturer != null && e.Manufacturer.ToLower().Contains(search))
                );
            }

            var equipment = await query
                .Select(e => new EquipmentDto
                {
                    Id = e.Id,
                    ClassroomId = e.ClassroomId,
                    ClassroomNumber = e.Classroom.Number,
                    EquipmentTypeId = e.EquipmentTypeId,
                    EquipmentTypeName = e.EquipmentType.Name,
                    InventoryNumber = e.InventoryNumber,
                    Manufacturer = e.Manufacturer,
                    Model = e.Model,
                    SerialNumber = e.SerialNumber,
                    PurchaseDate = e.PurchaseDate,
                    WarrantyEnd = e.WarrantyEnd,
                    Status = e.Status,
                    Notes = e.Notes,
                    Specifications = e.Specifications.Select(s => new SpecificationDto
                    {
                        Id = s.Id,
                        EquipmentId = s.EquipmentId,
                        CategoryId = s.CategoryId,
                        CategoryName = s.Category != null ? s.Category.Name : null,
                        CategoryDisplayName = s.Category != null ? s.Category.DisplayName : null,
                        Value = s.Value,
                        CustomName = s.CustomName
                    }).ToList()
                })
                .OrderBy(e => e.InventoryNumber)
                .ToListAsync();

            return Ok(equipment);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var equipment = await _context.Equipment
                .Include(e => e.Classroom)
                .Include(e => e.EquipmentType)
                .Include(e => e.Specifications)
                    .ThenInclude(s => s.Category)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (equipment == null)
                return NotFound();

            var result = new EquipmentDto
            {
                Id = equipment.Id,
                ClassroomId = equipment.ClassroomId,
                ClassroomNumber = equipment.Classroom.Number,
                EquipmentTypeId = equipment.EquipmentTypeId,
                EquipmentTypeName = equipment.EquipmentType.Name,
                InventoryNumber = equipment.InventoryNumber,
                Manufacturer = equipment.Manufacturer,
                Model = equipment.Model,
                SerialNumber = equipment.SerialNumber,
                PurchaseDate = equipment.PurchaseDate,
                WarrantyEnd = equipment.WarrantyEnd,
                Status = equipment.Status,
                Notes = equipment.Notes,
                Specifications = equipment.Specifications.Select(s => new SpecificationDto
                {
                    Id = s.Id,
                    EquipmentId = s.EquipmentId,
                    CategoryId = s.CategoryId,
                    CategoryName = s.Category != null ? s.Category.Name : null,
                    CategoryDisplayName = s.Category != null ? s.Category.DisplayName : null,
                    Value = s.Value,
                    CustomName = s.CustomName
                }).ToList()
            };

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> Create([FromBody] CreateEquipmentDto dto)
        {
            var classroom = await _context.Classrooms.FindAsync(dto.ClassroomId);
            if (classroom == null)
                return BadRequest(new { message = "Classroom not found" });

            var type = await _context.EquipmentTypes.FindAsync(dto.EquipmentTypeId);
            if (type == null)
                return BadRequest(new { message = "Equipment type not found" });

            // Check unique InventoryNumber
            if (await _context.Equipment.AnyAsync(e => e.InventoryNumber == dto.InventoryNumber))
                return BadRequest(new { message = "Inventory number already exists" });

            var equipment = new Equipment
            {
                ClassroomId = dto.ClassroomId,
                EquipmentTypeId = dto.EquipmentTypeId,
                InventoryNumber = dto.InventoryNumber,
                Manufacturer = dto.Manufacturer,
                Model = dto.Model,
                SerialNumber = dto.SerialNumber,
                PurchaseDate = dto.PurchaseDate,
                WarrantyEnd = dto.WarrantyEnd,
                Status = dto.Status,
                Notes = dto.Notes,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _context.Equipment.AddAsync(equipment);
            await _context.SaveChangesAsync();

            // Add specifications
            foreach (var specDto in dto.Specifications)
            {
                var spec = new EquipmentSpecification
                {
                    EquipmentId = equipment.Id,
                    CategoryId = specDto.CategoryId,
                    Value = specDto.Value,
                    CustomName = specDto.CustomName
                };
                await _context.EquipmentSpecifications.AddAsync(spec);
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = equipment.Id }, await GetEquipmentDto(equipment.Id));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEquipmentDto dto)
        {
            var equipment = await _context.Equipment
                .Include(e => e.Specifications)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (equipment == null)
                return NotFound();

            equipment.Manufacturer = dto.Manufacturer;
            equipment.Model = dto.Model;
            equipment.SerialNumber = dto.SerialNumber;
            equipment.PurchaseDate = dto.PurchaseDate;
            equipment.WarrantyEnd = dto.WarrantyEnd;
            equipment.Status = dto.Status;
            equipment.Notes = dto.Notes;
            equipment.UpdatedAt = DateTime.Now;

            // Update specifications (replace all)
            _context.EquipmentSpecifications.RemoveRange(equipment.Specifications);
            await _context.SaveChangesAsync();

            foreach (var specDto in dto.Specifications)
            {
                var spec = new EquipmentSpecification
                {
                    EquipmentId = equipment.Id,
                    CategoryId = specDto.CategoryId,
                    Value = specDto.Value,
                    CustomName = specDto.CustomName
                };
                await _context.EquipmentSpecifications.AddAsync(spec);
            }

            await _context.SaveChangesAsync();

            return Ok(await GetEquipmentDto(equipment.Id));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var equipment = await _context.Equipment
                .Include(e => e.Specifications)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (equipment == null)
                return NotFound();

            _context.Equipment.Remove(equipment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<EquipmentDto> GetEquipmentDto(int id)
        {
            var equipment = await _context.Equipment
                .Include(e => e.Classroom)
                .Include(e => e.EquipmentType)
                .Include(e => e.Specifications)
                    .ThenInclude(s => s.Category)
                .FirstOrDefaultAsync(e => e.Id == id);

            return new EquipmentDto
            {
                Id = equipment!.Id,
                ClassroomId = equipment.ClassroomId,
                ClassroomNumber = equipment.Classroom.Number,
                EquipmentTypeId = equipment.EquipmentTypeId,
                EquipmentTypeName = equipment.EquipmentType.Name,
                InventoryNumber = equipment.InventoryNumber,
                Manufacturer = equipment.Manufacturer,
                Model = equipment.Model,
                SerialNumber = equipment.SerialNumber,
                PurchaseDate = equipment.PurchaseDate,
                WarrantyEnd = equipment.WarrantyEnd,
                Status = equipment.Status,
                Notes = equipment.Notes,
                Specifications = equipment.Specifications.Select(s => new SpecificationDto
                {
                    Id = s.Id,
                    EquipmentId = s.EquipmentId,
                    CategoryId = s.CategoryId,
                    CategoryName = s.Category != null ? s.Category.Name : null,
                    CategoryDisplayName = s.Category != null ? s.Category.DisplayName : null,
                    Value = s.Value,
                    CustomName = s.CustomName
                }).ToList()
            };
        }
    }
}