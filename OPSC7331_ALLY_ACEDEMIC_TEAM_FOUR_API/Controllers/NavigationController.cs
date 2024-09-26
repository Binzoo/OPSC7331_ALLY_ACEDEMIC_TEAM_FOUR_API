using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Interface;
using OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Models;
using OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Models.DTO;
using OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Repositories;

namespace OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NavigationController : ControllerBase
    {
        private readonly IGenericRepository<SchoolNavigation> _repository;
        public NavigationController(IGenericRepository<SchoolNavigation> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var allNavigation = await _repository.GetAllAsync();
            return Ok(allNavigation);
        }

        [HttpGet("get-nav-by-id")]
        public async Task<IActionResult> GetNavById(int navId)
        {
            var allNavigation = await _repository.GetByIdAsync(navId);
            return Ok(allNavigation);
        }

        // [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] NavigationDTO navigationDTO)
        {
            SchoolNavigation navigation = new SchoolNavigation()
            {
                StartPlace = navigationDTO.StartPlace,
                EndPlace = navigationDTO.EndPlace,
                VideoUrl = navigationDTO.VideoUrl
            };
            await _repository.AddAsync(navigation);
            return Ok("Navigation Added Successfully.");
        }


    }
}