using Library.Core.Models.Items;
using Library.Core.Models;

namespace Library.Tests.ModelsTests.ItemsTests;

public class LoanTests
{
  [Fact]
  public void IsOverdue_ShouldReturnFalse_WhenDueDateIsInFuture()
  {
    var book = new Book("123", "Test", "Author", 123, 2024);
    var member = new Member("1", "Alice", "a@test.com", DateTime.Now, 10);
    var loan = new Loan(book, member, DateTime.Now, DateTime.Now.AddDays(14));

    Assert.False(loan.IsOverdue);
  }

  [Fact]
  public void IsOverdue_ShouldReturnTrue_WhenDueDateHasPassed()
  {
    var book = new Book("123", "Test", "Author", 123, 2024);
    var member = new Member("1", "Alice", "a@test.com", DateTime.Now, 10);
    var loan = new Loan(book, member, DateTime.Now, DateTime.Now.AddDays(-14));

    Assert.True(loan.IsOverdue);
  }

  [Fact]
  public void IsReturned_ShouldReturnTrue_WhenReturnDateIsSet()
  {
    var book = new Book("123", "Test", "Author", 123, 2024);
    var member = new Member("1", "Alice", "a@test.com", DateTime.Now, 10);
    var loan = new Loan(book, member, DateTime.Now, DateTime.Now) { ReturnDate = DateTime.Now };

    Assert.True(loan.IsReturned);
  }
}