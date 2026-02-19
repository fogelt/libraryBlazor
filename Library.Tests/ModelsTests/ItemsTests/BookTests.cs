using Library.Core.Models.Items;

namespace Library.Tests.ModelsTests.ItemsTests;

public class BookTests
{
  [Fact]
  public void Constructor_ShouldSetPropertiesCorrectly()
  {
    var book = new Book("978-91-0-012345-6", "Testbok", "Testförfattare", 123, 2024);

    Assert.Equal("978-91-0-012345-6", book.ISBN);
    Assert.Equal("Testbok", book.Title);
  }

  [Fact]
  public void IsAvailable_ShouldBeTrueForNewBook()
  {
    var book = new Book("978-91-0-012345-6", "Testbok", "Testförfattare", 123, 2024);
    Assert.True(book.IsAvailable);
  }
}