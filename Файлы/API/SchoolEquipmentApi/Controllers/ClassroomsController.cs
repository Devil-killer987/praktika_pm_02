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
    public class ClassroomsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClassroomsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? buildingId)
        {
            var query = _context.Classrooms
                .Include(c => c.Building)
                .Include(c => c.Equipment)
                .AsQueryable();

            if (buildingId.HasValue)
                query = query.Where(c => c.BuildingId == buildingId.Value);

            var classrooms = await query
                .Select(c => new ClassroomDto
                {
                    Id = c.Id,
                    BuildingId = c.BuildingId,
                    BuildingName = c.Building.Name,
                    Number = c.Number,
                    Floor = c.Floor,
                    Description = c.Description,
                    EquipmentCount = c.Equipment.Count
                })
                .OrderBy(c => c.Number)
                .ToListAsync();

            return Ok(classrooms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var classroom = await _context.Classrooms
                .Include(c => c.Building)
                .Include(c => c.Equipment)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (classroom == null)
                return NotFound();

            var result = new ClassroomDto
            {
                Id = classroom.Id,
                BuildingId = classroom.BuildingId,
                BuildingName = classroom.Building.Name,
                Number = classroom.Number,
                Floor = classroom.Floor,
                Description = classroom.Description,
                EquipmentCount = classroom.Equipment.Count
            };

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> Create([FromBody] CreateClassroomDto dto)
        {
            var building = await _context.Buildings.FindAsync(dto.BuildingId);
            if (building == null)
                return BadRequest(new { message = "Building not found" });

            var classroom = new Classroom
            {
                BuildingId = dto.BuildingId,
                Number = dto.Number,
                Floor = dto.Floor,
                Description = dto.Description
            };

            await _context.Classrooms.AddAsync(classroom);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = classroom.Id }, new ClassroomDto
            {
                Id = classroom.Id,
                BuildingId = classroom.BuildingId,
                BuildingName = building.Name,
                Number = classroom.Number,
                Floor = classroom.Floor,
                Description = classroom.Description
            });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateClassroomDto dto)
        {
            var classroom = await _context.Classrooms
                .Include(c => c.Building)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (classroom == null)
                return NotFound();

            classroom.Number = dto.Number;
            classroom.Floor = dto.Floor;
            classroom.Description = dto.Description;

            await _context.SaveChangesAsync();

            return Ok(new ClassroomDto
            {
                Id = classroom.Id,
                BuildingId = classroom.BuildingId,
                BuildingName = classroom.Building.Name,
                Number = classroom.Number,
                Floor = classroom.Floor,
                Description = classroom.Description
            });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var classroom = await _context.Classrooms
                .Include(c => c.Equipment)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (classroom == null)
                return NotFound();

            _context.Classrooms.Remove(classroom);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}