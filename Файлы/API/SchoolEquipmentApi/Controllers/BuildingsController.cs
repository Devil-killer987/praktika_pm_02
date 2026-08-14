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
    public class BuildingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BuildingsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var buildings = await _context.Buildings
                .Include(b => b.Classrooms)
                .Select(b => new BuildingDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Address = b.Address,
                    CreatedDate = b.CreatedDate,
                    ClassroomsCount = b.Classrooms.Count
                })
                .OrderBy(b => b.Name)
                .ToListAsync();

            return Ok(buildings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var building = await _context.Buildings
                .Include(b => b.Classrooms)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (building == null)
                return NotFound();

            var result = new BuildingDto
            {
                Id = building.Id,
                Name = building.Name,
                Address = building.Address,
                CreatedDate = building.CreatedDate,
                ClassroomsCount = building.Classrooms.Count
            };

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> Create([FromBody] CreateBuildingDto dto)
        {
            var building = new Building
            {
                Name = dto.Name,
                Address = dto.Address,
                CreatedDate = DateTime.Now
            };

            await _context.Buildings.AddAsync(building);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = building.Id }, new BuildingDto
            {
                Id = building.Id,
                Name = building.Name,
                Address = building.Address,
                CreatedDate = building.CreatedDate
            });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBuildingDto dto)
        {
            var building = await _context.Buildings.FindAsync(id);
            
            if (building == null)
                return NotFound();

            building.Name = dto.Name;
            building.Address = dto.Address;

            await _context.SaveChangesAsync();

            return Ok(new BuildingDto
            {
                Id = building.Id,
                Name = building.Name,
                Address = building.Address,
                CreatedDate = building.CreatedDate
            });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var building = await _context.Buildings
                .Include(b => b.Classrooms)
                .ThenInclude(c => c.Equipment)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (building == null)
                return NotFound();

            _context.Buildings.Remove(building);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}