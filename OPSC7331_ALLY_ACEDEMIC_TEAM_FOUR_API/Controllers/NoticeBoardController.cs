using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Interface;
using OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Models;
using OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Models.DTO;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;


namespace OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoticeBoardController : ControllerBase
    {
        private readonly IGenericRepository<NoticeBoard> _generic;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public NoticeBoardController(IGenericRepository<NoticeBoard> generic, IConfiguration configuration)
        {
            _generic = generic;
            _configuration = configuration;
            _httpClient = new HttpClient();
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

            // Trigger push notification after successfully adding the noticeboard
            await SendPushNotificationAsync(noticeBoard.Name, noticeBoard.Description);

            return Ok(new { message = "Notice board has been added.", imagePath = relativeImagePath });
        }

        private async Task SendPushNotificationAsync(string title, string message)
        {
            var notification = new
            {
                app_id = _configuration["OneSignal:AppId"],
                headings = new { en = title },
                contents = new { en = message },
                included_segments = new[] { "All" }
            };

            var json = JsonConvert.SerializeObject(notification);

            var content = new StringContent(json, Encoding.UTF8);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://onesignal.com/api/v1/notifications"),
                Headers =
        {
            { "Authorization", $"Basic {_configuration["OneSignal:ApiKey"]}" }
        },
                Content = content
            };

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
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

    }
}
