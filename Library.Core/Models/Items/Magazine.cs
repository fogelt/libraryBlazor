namespace Library.Core.Models.Items;

public class Magazine : LibraryItem
{
  public int IssueNumber { get; set; }

  protected Magazine() : base() { }
  public Magazine(string isbn, string title, string author, int issueNumber, int publishedYear, string description = "", string? imageUrl = null)
      : base(isbn, title, author, publishedYear, description, imageUrl)
  {
    IssueNumber = issueNumber;
  }
}