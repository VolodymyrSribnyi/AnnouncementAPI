using AnnouncementAPI.Application.DTOs;
using AnnouncementAPI.Application.Services;
using AnnouncementAPI.Domain.Interfaces;
using AnnouncementAPI.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnnouncementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementsController : ControllerBase
    {
        private readonly IAnnouncementsService _announcementsService;
        public AnnouncementsController(IAnnouncementsService announcementsService)
        {
            _announcementsService = announcementsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var announcements = await _announcementsService.GetAll();
            if(!announcements.Any())
                return NotFound();

            return Ok(announcements);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var announcement = await _announcementsService.Get(id);
            if (announcement == null)
                return NotFound(id);
            return Ok(announcement);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateAnnouncementDTO announcement)
        {
            var announcementCreated = await _announcementsService.Create(announcement);
            if (announcementCreated == null)
                return BadRequest();

            return CreatedAtAction(nameof(Get), new {id = announcementCreated.Id},announcementCreated);
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, AnnouncementDTO announcement)
        {
            await _announcementsService.Update(id,announcement);
            return Ok();
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _announcementsService.Delete(id);
            return Ok();
        }
    }
}
