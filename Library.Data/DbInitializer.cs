using Library.Core.Models;
using Library.Core.Models.Items;
using Microsoft.EntityFrameworkCore;

namespace Library.Data;

public static class DbInitializer
{
  public static async Task SeedData(IDbContextFactory<LibraryDbContext> contextFactory)
  {
    using var context = contextFactory.CreateDbContext();

    if (context.Database.IsRelational())
    {
      await context.Database.MigrateAsync();
    }
    else
    {
      await context.Database.EnsureCreatedAsync();
    }

    if (await context.Items.AnyAsync()) return;

    var book = new Book("978-0123", "The Clean Coder", "Robert C. Martin", 256, 2011, "A guide to professional conduct.");
    var dvd = new DVD("DVD-5544", "Interstellar", "Christopher Nolan", 10140, 2014, "Space exploration epic.");
    var magazine = new Magazine("MAG-001", "National Geographic", "Various", 1245, 2024, "Explore the world.");

    var member = new Member("M-001", "Alice Reader", "alice@example.com", DateTime.Now.AddMonths(-6), 85);

    var loan = new Loan(book, member, DateTime.Now.AddDays(-5), DateTime.Now.AddDays(9));
    book.IsAvailable = false;

    await context.Items.AddRangeAsync(book, dvd, magazine);
    await context.Members.AddAsync(member);
    await context.Loans.AddAsync(loan);

    await context.SaveChangesAsync();
  }
}