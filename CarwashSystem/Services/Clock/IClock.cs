namespace UI.Services.Clock
{
    public interface IClock
    {
        DateTime UtcNow { get; }
        DateTime NowNi { get; }
        DateTime ToNi(DateTime utc);
    }
}
