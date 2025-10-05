namespace Parking_Ticket_Management_App.Utils.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime TrimMilliseconds(this DateTime dateTime)
        {
            return new DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                dateTime.Hour,
                dateTime.Minute,
                dateTime.Second,
                0,
                dateTime.Kind
            );
        }
    }
}
