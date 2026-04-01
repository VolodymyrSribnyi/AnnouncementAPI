using AnnouncementAPI._01_Domain.Enums;

namespace AnnouncementAPI.Application.DTOs
{
    public class AnnouncementDTO
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }

        public Status Status { get; set; }

        public Category? Category { get; set; }

        public SubCategory? SubCategory { get; set; }
    }
}
