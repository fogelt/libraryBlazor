using Library.Core.Models.Items;

namespace Library.Tests.ModelsTests.ItemsTests;

public class DVDTests
{
  [Fact]
  public void Constructor_ShouldSetPropertiesCorrectly()
  {
    var dvd = new DVD("DVD-123", "Inception", "Nolan", 8880, 2010);

    Assert.Equal("DVD-123", dvd.ISBN);
    Assert.Equal("Inception", dvd.Title);
    Assert.Equal(8880, dvd.DurationInSeconds);
  }

  [Fact]
  public void IsAvailable_ShouldBeTrueForNewDVD()
  {
    var dvd = new DVD("DVD-123", "Inception", "Nolan", 8880, 2010);
    Assert.True(dvd.IsAvailable);
  }

}