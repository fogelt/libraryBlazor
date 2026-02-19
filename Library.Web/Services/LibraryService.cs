using Library.Core.Models;
using Library.Core.Models.Items;
using Library.Core.DTOs;
using Library.Core.Interfaces;

namespace Library.Web.Services;

public class LibraryService(
    IRepository<LibraryItem> itemRepo,
    IRepository<Member> memberRepo,
    IRepository<Loan> loanRepo)
{
    //Membership functions
    public async Task<List<Member>> GetAllMembersAsync() => [.. (await memberRepo.GetAllAsync("Loans.Item")).OrderBy(m => m.Name)];
    public async Task AddMemberAsync(Member member) => await memberRepo.AddAsync(member);
    public async Task DeleteMemberAsync(Member member) => await memberRepo.DeleteAsync(member.MemberId);

    //Loan functions
    public async Task<List<Loan>> GetAllLoansAsync() => [.. (await loanRepo.GetAllAsync("Member", "Item")).OrderBy(l => l.IsReturned).ThenBy(l => l.DueDate)];
    public async Task<bool> BorrowItemAsync(string isbn, string memberId)
    {
        var item = await itemRepo.GetByIdAsync(isbn);
        var member = await memberRepo.GetByIdAsync(memberId);

        if (item == null || member == null || !item.IsAvailable) return false;

        item.IsAvailable = false;
        await itemRepo.UpdateAsync(item);

        member.ActiveScore = Math.Min(100, member.ActiveScore + 5);
        await memberRepo.UpdateAsync(member);

        var loan = new Loan
        {
            ItemISBN = isbn,
            MemberId = memberId,
            LoanDate = DateTime.Now,
            DueDate = DateTime.Now.AddDays(14)
        };

        await loanRepo.AddAsync(loan);
        return true;
    }
    public async Task<bool> ReturnItemAsync(string loanId)
    {
        var loan = (await loanRepo.GetAllAsync("Item", "Member"))
                   .FirstOrDefault(l => l.Id == loanId);

        if (loan == null || loan.ReturnDate != null) return false;

        if (loan.Item != null)
        {
            loan.Item.IsAvailable = true;
            await itemRepo.UpdateAsync(loan.Item);
        }

        if (loan.Member != null)
        {
            loan.Member.ActiveScore = Math.Min(100, loan.Member.ActiveScore + 5);
            await memberRepo.UpdateAsync(loan.Member);
        }

        loan.ReturnDate = DateTime.Now;
        await loanRepo.UpdateAsync(loan);

        return true;
    }

    //LibraryItem functions
    public async Task<List<LibraryItem>> GetAllItemsAsync() => [.. (await itemRepo.GetAllAsync()).OrderBy(i => i.Title)];
    public async Task<LibraryItem?> GetLibraryItemAsync(string id) => await itemRepo.GetByIdAsync(id);

    public async Task<bool> AddItemAsync(LibraryItem item)
    {
        var existing = await itemRepo.GetByIdAsync(item.ISBN);
        if (existing != null) return false;

        item.IsAvailable = true;

        await itemRepo.AddAsync(item);
        return true;
    }

    public async Task<bool> AddItemFromDtoAsync(LibraryItemDto dto, string itemType)
    {
        LibraryItem finalizedItem = itemType switch
        {
            "Book" => new Book(dto.ISBN, dto.Title, dto.Author, dto.SpecialMetric, dto.PublishedYear, dto.Description, dto.ImageUrl),
            "DVD" => new DVD(dto.ISBN, dto.Title, dto.Author, dto.SpecialMetric, dto.PublishedYear, dto.Description, dto.ImageUrl),
            "Magazine" => new Magazine(dto.ISBN, dto.Title, dto.Author, dto.SpecialMetric, dto.PublishedYear, dto.Description, dto.ImageUrl),
            _ => throw new ArgumentException("Invalid item type")
        };

        return await AddItemAsync(finalizedItem);
    }

    public async Task<bool> DeleteItemAsync(string isbn)
    {
        var item = await itemRepo.GetByIdAsync(isbn);
        if (item == null || !item.IsAvailable) return false;

        await itemRepo.DeleteAsync(isbn);
        return true;
    }

    //Global stats
    public async Task<(int Total, int Loaned, string MVP)> GetStatisticsAsync()
    {
        var allItems = await itemRepo.GetAllAsync();
        var allLoans = await loanRepo.GetAllAsync();
        var allMembers = await memberRepo.GetAllAsync();

        int total = allItems.Count();
        int loaned = allItems.Count(i => !i.IsAvailable);

        var mvp = allMembers
            .OrderByDescending(m => allLoans.Count(l => l.MemberId == m.MemberId))
            .Select(m => m.Name)
            .FirstOrDefault() ?? "N/A";

        return (total, loaned, mvp);
    }
}