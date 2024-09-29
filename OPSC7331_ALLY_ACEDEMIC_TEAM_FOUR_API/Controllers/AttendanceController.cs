using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Data;
using OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Interface;
using OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Models;
using OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Models.DTO;

namespace OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IGenericRepository<Attendance> _generic;
        private readonly AppDbContext _context;
        private UserManager<AppUser> _userManger;

        public AttendanceController(IGenericRepository<Attendance> appDbContext, UserManager<AppUser> userManger, AppDbContext context)
        {
            _generic = appDbContext;
            _userManger = userManger;
            _context = context;

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // Get the user's email from the claim
            var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Find the user by their email
            var user = await _userManger.FindByEmailAsync(userEmail);

            var userAttendance = await _context.Attendances
                                               .Where(e => e.UserId == user.Id)
                                               .ToListAsync();


            return Ok(userAttendance);
        }



        [HttpPost]
        public async Task<IActionResult> RegisterAttendance([FromBody] AttendanceDTO attendanceDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var attendance = new Attendance
            {
                Date = attendanceDTO.Date,
                UserId = attendanceDTO.UserId,
                AttendanceStatus = attendanceDTO.AttendanceStatus.ToLower()
            };

            await _generic.AddAsync(attendance);
            return Ok("Attendance added for user.");
        }
    }
}
