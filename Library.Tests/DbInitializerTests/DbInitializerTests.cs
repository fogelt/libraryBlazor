using Microsoft.EntityFrameworkCore;
using Library.Data;
using Moq;

public class DbInitializerTests
{
  private IDbContextFactory<LibraryDbContext> CreateFactory()
  {
    var options = new DbContextOptionsBuilder<LibraryDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

    var mockFactory = new Mock<IDbContextFactory<LibraryDbContext>>();
    mockFactory.Setup(f => f.CreateDbContext())
               .Returns(() => new LibraryDbContext(options));

    return mockFactory.Object;
  }

  [Fact]
  public async Task SeedData_ShouldPopulateDatabase_WhenEmpty()
  {
    var factory = CreateFactory();

    await DbInitializer.SeedData(factory);

    using var context = factory.CreateDbContext();
    Assert.Equal(3, await context.Items.CountAsync());
    Assert.Single(await context.Members.ToListAsync());
    Assert.Single(await context.Loans.ToListAsync());
  }

  [Fact]
  public async Task SeedData_ShouldNotDuplicate_IfDataAlreadyExists()
  {
    var factory = CreateFactory();

    await DbInitializer.SeedData(factory);
    await DbInitializer.SeedData(factory);

    using var context = factory.CreateDbContext();
    var itemCount = await context.Items.CountAsync();
    Assert.Equal(3, itemCount);
  }

  [Fact]
  public async Task SeedData_ShouldSetBookAvailabilityToFalse_WhenLoaned()
  {
    var factory = CreateFactory();

    await DbInitializer.SeedData(factory);

    using var context = factory.CreateDbContext();
    var loanedBook = await context.Books.FirstOrDefaultAsync(b => b.ISBN == "978-0123");

    Assert.NotNull(loanedBook);
    Assert.False(loanedBook.IsAvailable);
  }
}