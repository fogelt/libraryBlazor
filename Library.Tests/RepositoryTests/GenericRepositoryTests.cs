using Library.Data.Repositories;
using Library.Data;
using Library.Core.Models.Items;
using Microsoft.EntityFrameworkCore;
using Moq;

public class GenericRepositoryTests
{
  private IDbContextFactory<LibraryDbContext> CreateFactory(string dbName)
  {
    var options = new DbContextOptionsBuilder<LibraryDbContext>()
        .UseInMemoryDatabase(databaseName: dbName)
        .Options;

    var mockFactory = new Mock<IDbContextFactory<LibraryDbContext>>();
    mockFactory.Setup(f => f.CreateDbContext()).Returns(() => new LibraryDbContext(options));

    return mockFactory.Object;
  }

  [Fact]
  public async Task AddAsync_ShouldSaveBookToDatabase()
  {
    var factory = CreateFactory("AddDb");
    var repository = new GenericRepository<Book>(factory);
    var book = new Book("1", "TestBook", "Test Author", 100, 2011);

    await repository.AddAsync(book);

    using var context = factory.CreateDbContext();
    var savedBook = await context.Books.FirstOrDefaultAsync(b => b.ISBN == "1");
    Assert.NotNull(savedBook);
    Assert.Equal("TestBook", savedBook.Title);
  }

  [Fact]
  public async Task SearchAsync_ShouldFindBooksByTitle()
  {
    var factory = CreateFactory("SearchDb");
    var repository = new GenericRepository<Book>(factory);
    await repository.AddAsync(new Book("1", "C# Guide", "Author A", 100, 2020));
    await repository.AddAsync(new Book("2", "Java Guide", "Author B", 120, 2021));

    var results = await repository.SearchAsync(b => b.Title.Contains("C#"));

    Assert.Single(results);
    Assert.Equal("C# Guide", results.First().Title);
  }

  [Fact]
  public async Task GetAllAsync_ShouldReturnAllBooks()
  {
    var factory = CreateFactory("GetAllDb");
    var repository = new GenericRepository<Book>(factory);
    await repository.AddAsync(new Book("1", "Book One", "Author A", 100, 2020));
    await repository.AddAsync(new Book("2", "Book Two", "Author B", 120, 2021));

    var allBooks = await repository.GetAllAsync();

    Assert.Equal(2, allBooks.Count());
  }

  [Fact]
  public async Task GetByIdAsync_ShouldReturnCorrectEntity()
  {
    var factory = CreateFactory("GetByIdDb");
    var repository = new GenericRepository<Book>(factory);
    var targetBook = new Book("42", "The Hitchhiker's Guide", "Douglas Adams", 200, 1979);
    await repository.AddAsync(targetBook);

    var result = await repository.GetByIdAsync("42");

    Assert.NotNull(result);
    Assert.Equal("The Hitchhiker's Guide", result.Title);
  }

  [Fact]
  public async Task UpdateAsync_ShouldModifyExistingRecord()
  {
    var factory = CreateFactory("UpdateDb");
    var repository = new GenericRepository<Book>(factory);
    var book = new Book("5", "Original Title", "Author", 100, 2020);
    await repository.AddAsync(book);

    book.Title = "Updated Title";
    await repository.UpdateAsync(book);

    using var context = factory.CreateDbContext();
    var updatedBook = await context.Books.FindAsync("5");
    Assert.Equal("Updated Title", updatedBook?.Title);
  }

  [Fact]
  public async Task DeleteAsync_ShouldRemoveRecordFromDatabase()
  {
    var factory = CreateFactory("DeleteDb");
    var repository = new GenericRepository<Book>(factory);
    var book = new Book("99", "To Be Deleted", "Author", 50, 2022);
    await repository.AddAsync(book);

    await repository.DeleteAsync("99");

    using var context = factory.CreateDbContext();
    var result = await context.Books.FindAsync("99");
    Assert.Null(result);
  }

  [Fact]
  public async Task DeleteAsync_WhenIdDoesNotExist_ShouldNotThrow()
  {
    var factory = CreateFactory("DeleteEmptyDb");
    var repository = new GenericRepository<Book>(factory);

    var exception = await Record.ExceptionAsync(() => repository.DeleteAsync("non-existent-id"));
    Assert.Null(exception);
  }
}