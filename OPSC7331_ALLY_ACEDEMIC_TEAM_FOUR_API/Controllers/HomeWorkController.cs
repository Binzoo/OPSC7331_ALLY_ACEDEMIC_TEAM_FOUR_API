using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Data;
using OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Interface;
using OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Models;
using OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Models.DTO;

namespace OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeWorkController : ControllerBase
    {
        private readonly IGenericRepository<HomeWork> _generic;
        private readonly AppDbContext _context;
        public UserManager<AppUser> _userManger;

        public HomeWorkController(IGenericRepository<HomeWork> generic, AppDbContext context, UserManager<AppUser> userManger)
        {
            _generic = generic;
            _context = context;
            _userManger = userManger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateHomeWork([FromBody] HomeWorkDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            HomeWork homeWork = new HomeWork()
            {
                Title = model.Title,
                Description = model.Description,
                ModuleID = model.ModuleID,
                DueDate = model.DueDate,
                LastUpdated = DateTime.UtcNow // Set LastUpdated to current time
            };

            await _generic.AddAsync(homeWork);
            return Ok(new
            {
                message = "Homework has been added."
            });
        }

        [Authorize(Roles = "student,lecturer")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized("User email not found.");
            }

            var user = await _userManger.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var degree = await _context.Degrees.FirstOrDefaultAsync(e => e.DegreeName == user.Degree);
            if (degree == null)
            {
                return NotFound("Degree not found.");
            }

            var modules = await _context.Modules.Where(e => e.DegreeID == degree.DegreeID).ToListAsync();
            if (modules == null || !modules.Any())
            {
                return NotFound("Modules not found.");
            }

            var moduleIds = modules.Select(m => m.ModuleID).ToList();
            var homeWorks = await _context.HomeWorks
                .Where(h => moduleIds.Contains(h.ModuleID))
                .Select(h => new HomeWorkDTO
                {
                    Title = h.Title,
                    Description = h.Description,
                    ModuleID = h.ModuleID,
                    DueDate = h.DueDate,
                    LastUpdated = h.LastUpdated // Include LastUpdated in the response
                })
                .ToListAsync();

            return Ok(homeWorks);
        }
    }
}
