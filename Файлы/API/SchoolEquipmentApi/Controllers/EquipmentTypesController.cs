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
    public class EquipmentTypesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EquipmentTypesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var types = await _context.EquipmentTypes
                .Include(t => t.Equipment)
                .Select(t => new EquipmentTypeDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Category = t.Category,
                    EquipmentCount = t.Equipment.Count
                })
                .OrderBy(t => t.Name)
                .ToListAsync();

            return Ok(types);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var type = await _context.EquipmentTypes
                .Include(t => t.Equipment)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (type == null)
                return NotFound();

            return Ok(new EquipmentTypeDto
            {
                Id = type.Id,
                Name = type.Name,
                Category = type.Category,
                EquipmentCount = type.Equipment.Count
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> Create([FromBody] CreateEquipmentTypeDto dto)
        {
            var type = new EquipmentType
            {
                Name = dto.Name,
                Category = dto.Category
            };

            await _context.EquipmentTypes.AddAsync(type);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = type.Id }, new EquipmentTypeDto
            {
                Id = type.Id,
                Name = type.Name,
                Category = type.Category
            });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEquipmentTypeDto dto)
        {
            var type = await _context.EquipmentTypes.FindAsync(id);
            
            if (type == null)
                return NotFound();

            type.Name = dto.Name;
            type.Category = dto.Category;

            await _context.SaveChangesAsync();

            return Ok(new EquipmentTypeDto
            {
                Id = type.Id,
                Name = type.Name,
                Category = type.Category
            });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var type = await _context.EquipmentTypes
                .Include(t => t.Equipment)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (type == null)
                return NotFound();

            if (type.Equipment.Any())
                return BadRequest(new { message = "Cannot delete type with existing equipment" });

            _context.EquipmentTypes.Remove(type);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}