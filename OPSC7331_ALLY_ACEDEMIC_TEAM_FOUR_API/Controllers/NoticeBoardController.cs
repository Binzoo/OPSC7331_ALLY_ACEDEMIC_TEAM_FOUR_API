using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Data;
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

        // [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AddNoticeBoard([FromForm] NoticeBoardDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.Image == null || model.Image.Length == 0)
            {
                return BadRequest("Please upload a valid image.");
            }

            var extension = Path.GetExtension(model.Image.FileName);
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

            if (!allowedExtensions.Contains(extension.ToLower()))
                return BadRequest("Only .jpg, .jpeg, .png, and .gif files are allowed.");

            var fileName = $"{Guid.NewGuid()}{extension}";
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await model.Image.CopyToAsync(stream);
            }
            var relativeImagePath = Path.Combine("/images", fileName);
            NoticeBoard noticeBoard = new NoticeBoard()
            {
                Name = model.Name,
                Description = model.Description,
                DateTime = model.DateTime,
                Image = relativeImagePath
            };

            await _generic.AddAsync(noticeBoard);

            return Ok(new { message = "Notice board has been added.", imagePath = relativeImagePath });
        }


        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateNoticeBoard(int id, [FromForm] NoticeBoardDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var noticeb = await _generic.GetByIdAsync(id);

            if (noticeb == null)
            {
                return NotFound();
            }

            noticeb.Name = model.Name;
            noticeb.Description = model.Description;
            noticeb.DateTime = model.DateTime;

            if (model.Image != null && model.Image.Length > 0)
            {
                var extension = Path.GetExtension(model.Image.FileName);
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

                if (!allowedExtensions.Contains(extension.ToLower()))
                {
                    return BadRequest("Only .jpg, .jpeg, .png, and .gif files are allowed.");
                }

                if (!string.IsNullOrEmpty(noticeb.Image))
                {
                    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", noticeb.Image.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                var fileName = $"{Guid.NewGuid()}{extension}";
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }

                noticeb.Image = Path.Combine("/images", fileName);
            }
            await _generic.UpdateAsync(noticeb);

            return Ok("Notice board has been updated.");
        }

        [Authorize(Roles = "admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteNoticeBoard(int id)
        {
            var noticeBoard = await _generic.GetByIdAsync(id);

            if (noticeBoard == null)
            {
                return NotFound("Notice board not found.");
            }
            if (!string.IsNullOrEmpty(noticeBoard.Image))
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", noticeBoard.Image.TrimStart('/'));

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            await _generic.DeleteAsync(noticeBoard);

            return Ok("Notice board has been deleted successfully.");
        }

    }
}
