using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Interface;
using OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Models;
using OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Models.DTO;

namespace OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MultiMediaController : ControllerBase
    {
        private readonly IGenericRepository<MultiMedia> _generic;

        public MultiMediaController(IGenericRepository<MultiMedia> generic)
        {
            _generic = generic;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var allMedia = await _generic.GetAllAsync();
            return Ok(allMedia);
        }


        [HttpPost]
        public async Task<IActionResult> CreateMedia([FromForm] MultiMediaDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get the file extension
            var extension = Path.GetExtension(model.File.FileName);
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".pdf", ".zip" };

            // Check if the uploaded file has an allowed extension
            if (!allowedExtensions.Contains(extension.ToLower()))
            {
                return BadRequest("Only .jpg, .jpeg, .png, .gif, .pdf, and .zip files are allowed.");
            }

            // Generate a new unique file name
            var fileName = $"{Guid.NewGuid()}{extension}";
            var fileDirectory = "wwwroot/files";

            // Use "images" subfolder for images and "files" subfolder for other types
            if (new[] { ".jpg", ".jpeg", ".png", ".gif" }.Contains(extension.ToLower()))
            {
                fileDirectory = "wwwroot/images";
            }

            // Ensure the directory exists
            var fullDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), fileDirectory);
            if (!Directory.Exists(fullDirectoryPath))
            {
                Directory.CreateDirectory(fullDirectoryPath);  // Create the directory if it doesn't exist
            }

            var filePath = Path.Combine(fullDirectoryPath, fileName);

            // Save the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }

            // Determine relative file path
            var relativeFilePath = Path.Combine(fileDirectory.Replace("wwwroot", ""), fileName);

            // Create new multimedia entity
            MultiMedia multiMedia = new MultiMedia()
            {
                Title = model.Title,
                File = relativeFilePath,
                ModuleName = model.ModuleName
            };

            // Save multimedia entity to database
            await _generic.AddAsync(multiMedia);

            return Ok(new
            {
                message = "Multimedia has been added."
            });
        }


    }
}