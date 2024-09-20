using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Interface;
using OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Models;
using OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Models.DTO;
using System.Reflection.Metadata.Ecma335;

namespace OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoticeBoardController : ControllerBase
    {
        private readonly IGenericRepository<NoticeBoard> _generic;
        public NoticeBoardController(IGenericRepository<NoticeBoard> generic)
        {
            _generic = generic;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var allNoticeBoard = await _generic.GetAllAsync();
            return Ok(allNoticeBoard);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AddNoticeBoard([FromBody] NoticeBoardDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  
            }

            NoticeBoard noticeBoard = new NoticeBoard()
            {
                Name = model.Name,
                Description = model.Description,
                DateTime = model.DateTime
            };

            await _generic.AddAsync(noticeBoard);
            return Ok("Notice board has been added.");
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateNoticeBoard(int id, [FromBody] NoticeBoardDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var noticeb = await _generic.GetByIdAsync(id);
                
            if(noticeb == null)
            {
                return NotFound();
            }
            noticeb.Name = model.Name;
            noticeb.Description = model.Description;
            noticeb.DateTime = model.DateTime;
           
            await _generic.UpdateAsync(noticeb);
            return Ok("Notice board has been updated.");
        }


        [Authorize(Roles = "admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteNoticeBoard(int id)
        {
            var noticeBoard = await _generic.GetByIdAsync(id);

            if(noticeBoard == null)
            {
                return NotFound("Director Not Found.");
            }
            await _generic.DeleteAsync(noticeBoard);
            return Ok("Notice board has been deleted Successfully.");
        }
    }
}
