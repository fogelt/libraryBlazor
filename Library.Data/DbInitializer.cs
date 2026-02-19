using Library.Core.Models;
using Library.Core.Models.Items;
using Microsoft.EntityFrameworkCore;

namespace Library.Data;

public static class DbInitializer
{
  public static async Task SeedData(IDbContextFactory<LibraryDbContext> contextFactory)
  {
    using var context = contextFactory.CreateDbContext();

    //  Safety Check for Tests
    if (context.Database.IsRelational())
    {
      await context.Database.MigrateAsync();
    }
    else
    {
      await context.Database.EnsureCreatedAsync();
    }

    if (await context.Items.AnyAsync()) return;

    var book = new Book(
        "978-0132350884",
        "The Clean Coder",
        "Robert C. Martin",
        256,
        2011,
        "A comprehensive guide to professional conduct in software engineering. Robert Martin introduces the disciplines, techniques, and tools of true software craftsmanship, covering everything from estimating and coding to refactoring and testing.",
        "https://m.media-amazon.com/images/I/41nUxzDHD-L._SY445_SX342_ML2_.jpg"
    );

    var dvd = new DVD(
        "DVD-INCEP-2010",
        "Inception",
        "Christopher Nolan",
        8880,
        2010,
        "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O. A mind-bending thriller that explores the layers of reality and subconsciousness.",
        "https://m.media-amazon.com/images/I/912AErFSBHL._AC_SL1500_.jpg"
    );

    var magazine = new Magazine(
        "MAG-NG-2024",
        "National Geographic",
        "National Geographic Society",
        1245,
        2024,
        "The April 2024 issue explores the hidden depths of the Amazon rainforest, featuring award-winning photography and in-depth articles on biodiversity, indigenous cultures, and the impacts of climate change on the world's largest tropical forest.",
        "https://www.engelskatidskrifter.se//Files/10/168000/168032/ProductPhotos/Source/2330643983.jpg"
    );

    var member = new Member(
        "M-001",
        "Alice Reader",
        "alice@example.com",
        DateTime.Now.AddMonths(-6),
        85
    );

    var loan = new Loan(book, member, DateTime.Now.AddDays(-5), DateTime.Now.AddDays(9));
    book.IsAvailable = false;

    await context.Items.AddRangeAsync(book, dvd, magazine);
    await context.Members.AddAsync(member);
    await context.Loans.AddAsync(loan);

    await context.SaveChangesAsync();
  }
}