using AnnouncementAPI.Domain.Interfaces;
using AnnouncementAPI.Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AnnouncementAPI.Infrastructure.Repositories
{
    public class AnnouncementRepository : IAnnouncementRepository
    {
        private readonly AnnouncementsDbContext _context;
        public AnnouncementRepository(AnnouncementsDbContext context)
        {
            _context = context;
        }

        public async Task<Announcement> Create(Announcement newAnnouncement)
        {
            await _context.Database.ExecuteSqlAsync($"EXEC insertData @id={newAnnouncement.Id},@Title={newAnnouncement.Title},@Description={newAnnouncement.Description},@CreatedAt={newAnnouncement.CreatedDate},@Status={newAnnouncement.Status},@Category={newAnnouncement.Category},@SubCategory={newAnnouncement.SubCategory}");
            await _context.SaveChangesAsync();
            return newAnnouncement;
        }

        public async Task Delete(Guid id)
        {
            await _context.Database.ExecuteSqlAsync($"EXEC deleteData @Id={id}");
            await _context.SaveChangesAsync();
        }

        public async Task<List<Announcement>> GetAll()
        {
            _context.Announcements.AsNoTracking();
            var announcemenets = await _context.Announcements.FromSqlRaw("EXEC selectData").ToListAsync();
            return announcemenets;
        }

        public async Task<Announcement> Get(Guid id)
        {
            var rows = await _context.Announcements
                .FromSqlInterpolated($"EXEC selectDataById @Id={id}")
                .ToListAsync();

            return rows.FirstOrDefault();
        }

        public async Task Update(Announcement announcement)
        {
            await _context.Database.ExecuteSqlAsync($"EXEC updateData @id={announcement.Id},@Title={announcement.Title},@Description={announcement.Description},@CreatedAt={announcement.CreatedDate},@Status={announcement.Status},@Category={announcement.Category},@SubCategory={announcement.SubCategory}");
            await _context.SaveChangesAsync();
        }
    }
}
