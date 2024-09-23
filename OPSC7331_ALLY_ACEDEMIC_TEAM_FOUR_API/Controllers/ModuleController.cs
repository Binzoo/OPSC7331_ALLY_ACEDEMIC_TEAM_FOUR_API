using Microsoft.AspNetCore.Http;
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
    public class ModuleController : ControllerBase
    {
        private readonly IGenericRepository<Module> _generic;
        private readonly AppDbContext _context;
        public ModuleController(IGenericRepository<Module> generic, AppDbContext context)
        {
            _generic = generic;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var all = await _context.Modules.Include(e => e.HomeWorks).ToListAsync();
            // var allModule = await _generic.GetAllAsync();
            return Ok(all);
        }


        // [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AddDegree([FromBody] ModuleDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Module module = new Module()
            {
                ModuleName = model.ModuleName,
                Credits = model.Credits,
                DegreeID = model.DegreeID
            };

            await _generic.AddAsync(module);
            return Ok("Module has been added.");
        }
    }
}
