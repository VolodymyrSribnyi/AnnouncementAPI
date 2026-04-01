using AnnouncementAPI.Application.DTOs;
using AnnouncementAPI.Domain.Models;

namespace AnnouncementAPI.Domain.Interfaces
{
    public interface IAnnouncementsService
    {
        Task<AnnouncementDTO> Create(CreateAnnouncementDTO newAnnouncement);
        Task Update(Guid id,AnnouncementDTO announcement);
        Task<List<AnnouncementDTO>> GetAll();
        Task<AnnouncementDTO?> Get(Guid id);
        Task Delete(Guid id);
    }
}
