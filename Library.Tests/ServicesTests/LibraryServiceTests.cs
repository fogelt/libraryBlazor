using Moq;
using Library.Web.Services;
using Library.Core.Interfaces;
using Library.Core.Models;
using Library.Core.Models.Items;
using Library.Core.DTOs;

public class LibraryServiceTests
{
  private readonly Mock<IGenericRepository<LibraryItem>> _itemRepoMock;
  private readonly Mock<IGenericRepository<Member>> _memberRepoMock;
  private readonly Mock<IGenericRepository<Loan>> _loanRepoMock;
  private readonly LibraryService _service;

  public LibraryServiceTests()
  {
    _itemRepoMock = new Mock<IGenericRepository<LibraryItem>>();
    _memberRepoMock = new Mock<IGenericRepository<Member>>();
    _loanRepoMock = new Mock<IGenericRepository<Loan>>();

    _service = new LibraryService(
        _itemRepoMock.Object,
        _memberRepoMock.Object,
        _loanRepoMock.Object);
  }

  [Fact]
  public async Task BorrowItemAsync_WhenAvailable_ShouldCreateLoanAndDecreaseAvailability()
  {
    var isbn = "123";
    var memberId = "M1";
    var item = new Book(isbn, "Test", "Author", 100, 2020) { IsAvailable = true };
    var member = new Member(memberId, "User", "test@test.com", DateTime.Now, 50);

    _itemRepoMock.Setup(r => r.GetByIdAsync(isbn)).ReturnsAsync(item);
    _memberRepoMock.Setup(r => r.GetByIdAsync(memberId)).ReturnsAsync(member);

    var result = await _service.BorrowItemAsync(isbn, memberId);

    Assert.True(result);
    Assert.False(item.IsAvailable);
    Assert.Equal(55, member.ActiveScore);
    _itemRepoMock.Verify(r => r.UpdateAsync(item), Times.Once);
    _loanRepoMock.Verify(r => r.AddAsync(It.IsAny<Loan>()), Times.Once);
  }

  [Fact]
  public async Task BorrowItemAsync_WhenItemAlreadyLoaned_ShouldReturnFalse()
  {
    var isbn = "123";
    var item = new Book(isbn, "Test", "Author", 100, 2020) { IsAvailable = false };
    _itemRepoMock.Setup(r => r.GetByIdAsync(isbn)).ReturnsAsync(item);

    var result = await _service.BorrowItemAsync(isbn, "any-id");

    Assert.False(result);
    _loanRepoMock.Verify(r => r.AddAsync(It.IsAny<Loan>()), Times.Never);
  }

  [Fact]
  public async Task AddItemFromDtoAsync_ShouldMapCorrectType_AndCallAdd()
  {
    var dto = new AddLibraryItemDto { ISBN = "DVD-1", Title = "Movie", Author = "Director" };
    _itemRepoMock.Setup(r => r.GetByIdAsync(dto.ISBN)).ReturnsAsync((LibraryItem?)null);

    await _service.AddItemFromDtoAsync(dto, "DVD");

    _itemRepoMock.Verify(r => r.AddAsync(It.Is<LibraryItem>(i => i is DVD && i.ISBN == "DVD-1")), Times.Once);
  }

  [Fact]
  public async Task GetStatisticsAsync_ShouldCalculateCorrectTotals()
  {
    var items = new List<LibraryItem>
        {
            new Book("1", "B1", "A", 1, 1) { IsAvailable = true },
            new Book("2", "B2", "A", 1, 1) { IsAvailable = false }
        };
    _itemRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(items);
    _loanRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Loan>());
    _memberRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Member>());

    var stats = await _service.GetStatisticsAsync();

    Assert.Equal(2, stats.Total);
    Assert.Equal(1, stats.Loaned);
  }
}