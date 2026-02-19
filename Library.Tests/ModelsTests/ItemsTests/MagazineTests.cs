using Library.Core.Models.Items;

namespace Library.Tests.ModelsTests.ItemsTests;

public class MagazineTests
{
  [Fact]
  public void Constructor_ShouldSetPropertiesCorrectly()
  {
    var magazine = new Magazine("ISSN-456", "Tech Monthly", "Tech Publisher", 45, 2024);

    Assert.Equal("ISSN-456", magazine.ISBN);
    Assert.Equal("Tech Monthly", magazine.Title);
    Assert.Equal(45, magazine.IssueNumber);
  }

  [Fact]
  public void IsAvailable_ShouldBeTrueForNewMagazine()
  {
    var magazine = new Magazine("ISSN-456", "Tech Monthly", "Tech Publisher", 45, 2024);
    Assert.True(magazine.IsAvailable);
  }

}