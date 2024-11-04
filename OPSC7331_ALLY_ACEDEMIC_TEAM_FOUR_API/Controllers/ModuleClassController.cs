using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Interface;
using OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Models;
using OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Models.DTO;

namespace OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleClassController : ControllerBase
    {
        private readonly IGenericRepository<ModuleClass> _generic;

        public ModuleClassController(IGenericRepository<ModuleClass> generic)
        {
            _generic = generic;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ModuleClassDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please input all required fields.");
            }

            try
            {
                // Parse string values into TimeOnly and DateOnly
                var startTime = TimeOnly.ParseExact(model.StartTime, "HH:mm"); // Handle the string format
                var endTime = TimeOnly.ParseExact(model.EndTime, "HH:mm");

                var moduleclass = new ModuleClass
                {
                    StartTime = startTime,
                    EndTime = endTime,
                    ModuleId = model.ModuleId,
                    Date = model.Date
                };

                await _generic.AddAsync(moduleclass);
                return Ok("Module has been added.");
            }
            catch (FormatException ex)
            {
                return BadRequest($"Invalid time or date format: {ex.Message}");
            }
        }

    }
}