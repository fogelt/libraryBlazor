using Library.Core.Models;
using Library.Core.Models.Items;
using Library.Core.DTOs;

namespace Library.Core.Interfaces;

public interface ILibraryService
{
  // Membership
  Task<List<Member>> GetAllMembersAsync();
  Task AddMemberAsync(Member member);
  Task AddMemberFromDtoAsync(AddMemberDto dto);
  Task DeleteMemberAsync(Member member);

  // Loans
  Task<List<Loan>> GetAllLoansAsync();
  Task<bool> BorrowItemAsync(string isbn, string memberId);
  Task<bool> ReturnItemAsync(string loanId);

  // LibraryItems
  Task<List<LibraryItem>> GetAllItemsAsync();
  Task<LibraryItem?> GetLibraryItemAsync(string id);
  Task<bool> AddItemAsync(LibraryItem item);
  Task<bool> AddItemFromDtoAsync(AddLibraryItemDto dto, string itemType);
  Task<bool> DeleteItemAsync(string isbn);

  // Stats
  Task<(int Total, int Loaned, string MVP)> GetStatisticsAsync();
}