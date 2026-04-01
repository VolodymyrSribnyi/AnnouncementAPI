using AnnouncementAPI.Domain.Models;

namespace AnnouncementAPI.Domain.Interfaces
{
    public interface IAnnouncementRepository
    {
        Task<Announcement> Create(Announcement newAnnouncement);
        Task Update(Announcement announcement);
        Task<List<Announcement>> GetAll();
        Task<Announcement> Get(Guid id);
        Task Delete(Guid id);
    }
}
