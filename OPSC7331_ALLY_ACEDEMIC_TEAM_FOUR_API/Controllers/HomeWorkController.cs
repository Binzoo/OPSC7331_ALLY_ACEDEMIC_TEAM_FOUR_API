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
                DueDate = model.DueDate
            };

            await _generic.AddAsync(homeWork);
            return Ok(new
            {
                message = "Home work has been added."
            });
        }

        [Authorize(Roles = "student")]
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
                .ToListAsync();

            return Ok(homeWorks);
        }
    }
}
