using Library.Core.Utils;

namespace Library.Tests;

public class TimeFormatterTests
{
  [Fact]
  public void FormatDuration_SecondsOnly_ReturnsMinutesAndSeconds()
  {
    int seconds = 45;
    string result = TimeFormatter.FormatDuration(seconds);
    Assert.Equal("00m 45s", result);
  }

  [Fact]
  public void FormatDuration_MoreThanOneMinute_ReturnsMinutesAndSeconds()
  {
    int seconds = 125;
    string result = TimeFormatter.FormatDuration(seconds);
    Assert.Equal("02m 05s", result);
  }

  [Fact]
  public void FormatDuration_MoreThanOneHour_ReturnsHoursMinutesAndSeconds()
  {
    int seconds = 3661;
    string result = TimeFormatter.FormatDuration(seconds);
    Assert.Equal("01h 01m", result);
  }

  [Theory]
  [InlineData(0, "00m 00s")]
  [InlineData(59, "00m 59s")]
  [InlineData(3600, "01h 00m")]
  public void FormatDuration_MultipleInputs_ReturnCorrectFormat(int seconds, string expected)
  {
    string result = TimeFormatter.FormatDuration(seconds);
    Assert.Equal(expected, result);
  }
}