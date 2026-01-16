namespace UI.Services.Clock
{
    public sealed class Clock :IClock
    {
        private static readonly TimeZoneInfo Tz =
        TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time");

        public DateTime UtcNow => DateTime.UtcNow;

        public DateTime NowNi => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, Tz);

        public DateTime ToNi(DateTime utc) => TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(utc, DateTimeKind.Utc), Tz);
    }
}
