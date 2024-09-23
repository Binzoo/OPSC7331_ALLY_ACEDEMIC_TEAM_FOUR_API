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
    public class DegreeController : ControllerBase
    {
        private readonly IGenericRepository<Degree> _generic;
        private readonly AppDbContext _context;
        private UserManager<AppUser> _userManager;
        public DegreeController(IGenericRepository<Degree> generic, AppDbContext context, UserManager<AppUser> userManager)
        {
            _generic = generic;
            _context = context;
            _userManager = userManager;
        }

        // [Authorize(Roles = "Student")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // var user = User.FindFirst("sub").Value;
            // System.Console.WriteLine(user);
            var degree = await _context.Degrees.ToListAsync();
            return Ok(degree);
        }


        [Authorize(Roles = "STUDENT")]
        [HttpGet("get-student-degree-info")]
        public async Task<IActionResult> GetStudnetDegreeInfo()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized("User email not found.");
            }
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var degree = await _context.Degrees.Where(e => e.DegreeName == user.Degree).Include(d => d.Modules)!.ThenInclude(e => e.HomeWorks).ToListAsync();

            return Ok(degree);
        }


        // [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AddDegree([FromBody] DegreeDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Degree noticeBoard = new Degree()
            {
                DegreeName = model.DegreeName
            };

            await _generic.AddAsync(noticeBoard);
            return Ok("Degree has been added.");
        }
    }
}
