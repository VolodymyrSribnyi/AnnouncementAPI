namespace Client.Models
{
    public class CreateAnnouncementDTO
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int Status { get; set; }

        public int? Category { get; set; }

        public int? SubCategory { get; set; }
    }
}
