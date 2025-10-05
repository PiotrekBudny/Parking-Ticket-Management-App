namespace Parking_Ticket_Management_App.Utils
{
    public interface ISystemDateTimeProvider
    {
        DateTime UtcNow { get; }
    }

    public class SystemDateTimeProvider : ISystemDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
