using Library.Core.Models;
using Library.Core.Models.Items;
using Library.Core.Interfaces;

namespace Library.Core.Services;

public class LibraryService : ILibraryService
{
    private readonly List<LibraryItem> _items;
    private readonly List<Member> _members;

    // --- Member methods ---
    public List<Member> GetAllMembers() => [.. _members.OrderBy(m => m.Name)];

    public void AddMember(Member member)
    {
        _members.Add(member);
    }

    // --- Item methods ---
    public List<LibraryItem> SearchItems(string searchTerm) =>
        [.. _items.Where(i => i.Matches(searchTerm))];

    public List<LibraryItem> SortItemsAlphabetically() =>
        [.. _items.OrderBy(b => b.Title)];

    public List<LibraryItem> SortItemsReleaseDate() =>
        [.. _items.OrderBy(b => b.PublishedYear)];

    public int GetItemsCount() => _items.Count();
    public List<LibraryItem> GetAllItems() => [.. _items.OrderBy(i => i.Title)];
    public void AddItem(LibraryItem item)
    {
        _items.Add(item);
    }

    // --- Core logic ---
    public bool BorrowItem(string isbn, string memberId)
    {
        var item = _items.FirstOrDefault(i => i.ISBN == isbn);
        var member = _members.FirstOrDefault(m => m.MemberId == memberId);

        if (item == null) throw new ArgumentException($"Item with ISBN {isbn} not found.");
        if (member == null) throw new ArgumentException($"Member with ID {memberId} not found.");
        if (!item.IsAvailable) throw new InvalidOperationException($"Item '{item.Title}' is already on loan.");

        item.IsAvailable = false;
        member.Inventory.Add(item);

        return true;
    }

    public bool ReturnItem(string isbn, string memberId)
    {
        var member = _members.FirstOrDefault(m => m.MemberId == memberId);
        if (member == null) throw new ArgumentException("Member not found.");

        var item = member.Inventory.FirstOrDefault(i => i.ISBN == isbn);
        if (item == null) throw new InvalidOperationException("This member does not have this item in their inventory.");

        item.IsAvailable = true;
        member.Inventory.Remove(item);

        return true;
    }

    // --- Statistics ---
    public int ItemsOnLoanCount() => _items.Count(i => !i.IsAvailable);

    public string MostActiveMember() =>
        _members.MaxBy(m => m.ActiveScore)?.Name ?? "No members found";

    public (int Total, int Loaned, string MVP) GetStatistics()
    {
        return (GetItemsCount(), ItemsOnLoanCount(), MostActiveMember());
    }
}