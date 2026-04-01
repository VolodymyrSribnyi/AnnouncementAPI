using AnnouncementAPI._01_Domain.Exceptions;
using AnnouncementAPI.Application.DTOs;
using AnnouncementAPI.Domain.Interfaces;
using AnnouncementAPI.Domain.Models;

namespace AnnouncementAPI.Application.Services
{
    public class AnnouncementsService : IAnnouncementsService
    {
        private readonly IAnnouncementRepository _announcementRepository;
        public AnnouncementsService(IAnnouncementRepository announcementRepository)
        {
            _announcementRepository = announcementRepository;
        }

        public async Task<AnnouncementDTO> Create(CreateAnnouncementDTO newAnnouncement)
        {
            var newId = Guid.NewGuid();
            var newDate = DateTime.UtcNow;

            var announcement = new Announcement
            {
                Id = newId,
                Title = newAnnouncement.Title,
                Description = newAnnouncement.Description,
                Status = newAnnouncement.Status,
                Category = newAnnouncement.Category,
                SubCategory = newAnnouncement.SubCategory,
                CreatedDate = newDate
            };

            await _announcementRepository.Create(announcement);

            var dto = new AnnouncementDTO
            {
                Id = announcement.Id,
                Title = announcement.Title,
                Description = announcement.Description,
                Status = announcement.Status,
                Category = newAnnouncement.Category,
                SubCategory = newAnnouncement.SubCategory,
                CreatedDate = newDate
            };

            return dto;
        }

        public async Task Delete(Guid id)
        {
            var announcement = await _announcementRepository.Get(id);
            if (announcement == null)
                throw new NotFoundException(id);

            await _announcementRepository.Delete(id);
        }

        public async Task<List<AnnouncementDTO>> GetAll()
        {
            var announcements = await _announcementRepository.GetAll();

            if (announcements.Any())
            {
                var announcementsDTO = announcements.Select(a => new AnnouncementDTO
                {
                    Id = a.Id,
                    Title = a.Title,
                    Description = a.Description,
                    Status = a.Status,
                    Category = a.Category,
                    SubCategory = a.SubCategory,
                    CreatedDate = a.CreatedDate
                });
                return announcementsDTO.ToList();
            }
            return new List<AnnouncementDTO>();
        }

        public async Task<AnnouncementDTO?> Get(Guid id)
        {
            var announcement = await _announcementRepository.Get(id);
            if (announcement == null)
            {
                throw new NotFoundException(id);
            }
            var announcementDTO = new AnnouncementDTO
            {
                Id = announcement.Id,
                Title = announcement.Title,
                Description = announcement.Description,
                Status = announcement.Status,
                Category = announcement.Category,
                SubCategory = announcement.SubCategory,
                CreatedDate = announcement.CreatedDate
            };
            return announcementDTO;
        }

        public async Task Update(Guid id, AnnouncementDTO announcementDto)
        {
            var announcementToUpdate = await _announcementRepository.Get(id);
            if (announcementToUpdate == null)
                throw new NotFoundException(id);

            if (announcementToUpdate != null)
            {
                announcementToUpdate.Title = announcementDto.Title;
                announcementToUpdate.Description = announcementDto.Description;
                announcementToUpdate.Status = announcementDto.Status;
                announcementToUpdate.Category = announcementDto.Category;
                announcementToUpdate.SubCategory = announcementDto.SubCategory;

                await _announcementRepository.Update(announcementToUpdate);
            }
        }
    }
}
