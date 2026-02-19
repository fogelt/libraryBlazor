namespace Library.Core.Utils;

public static class TimeFormatter
{
  public static string FormatDuration(int totalSeconds)
  {
    TimeSpan t = TimeSpan.FromSeconds(totalSeconds);

    if (t.TotalHours >= 1)
    {
      return string.Format("{0:D2}h {1:D2}m",
          (int)t.TotalHours,
          t.Minutes);
    }

    return string.Format("{0:D2}m {1:D2}s",
        t.Minutes,
        t.Seconds);
  }
}