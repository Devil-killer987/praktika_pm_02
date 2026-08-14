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
    public class SpecificationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SpecificationsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories([FromQuery] int? equipmentTypeId)
        {
            var query = _context.SpecificationCategories
                .Include(c => c.EquipmentType)
                .AsQueryable();

            if (equipmentTypeId.HasValue)
                query = query.Where(c => c.EquipmentTypeId == equipmentTypeId.Value);

            var categories = await query
                .Select(c => new SpecificationCategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    DisplayName = c.DisplayName,
                    Unit = c.Unit,
                    EquipmentTypeId = c.EquipmentTypeId,
                    EquipmentTypeName = c.EquipmentType.Name
                })
                .OrderBy(c => c.DisplayName)
                .ToListAsync();

            return Ok(categories);
        }

        [HttpGet("categories/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _context.SpecificationCategories
                .Include(c => c.EquipmentType)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return NotFound();

            return Ok(new SpecificationCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                DisplayName = category.DisplayName,
                Unit = category.Unit,
                EquipmentTypeId = category.EquipmentTypeId,
                EquipmentTypeName = category.EquipmentType.Name
            });
        }

        [HttpPost("categories")]
        [Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateSpecificationCategoryDto dto)
        {
            var type = await _context.EquipmentTypes.FindAsync(dto.EquipmentTypeId);
            if (type == null)
                return BadRequest(new { message = "Equipment type not found" });

            var category = new SpecificationCategory
            {
                Name = dto.Name,
                DisplayName = dto.DisplayName,
                Unit = dto.Unit,
                EquipmentTypeId = dto.EquipmentTypeId
            };

            await _context.SpecificationCategories.AddAsync(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, new SpecificationCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                DisplayName = category.DisplayName,
                Unit = category.Unit,
                EquipmentTypeId = category.EquipmentTypeId,
                EquipmentTypeName = type.Name
            });
        }

        [HttpDelete("categories/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.SpecificationCategories
                .Include(c => c.Specifications)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return NotFound();

            _context.SpecificationCategories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}