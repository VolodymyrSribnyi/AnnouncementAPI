namespace AnnouncementAPI._01_Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(Guid id)
            : base($"Entity with {id} not found")
        { }
    }
}
