namespace Library.Core.Models.Items;

public class DVD : LibraryItem
{
  public int DurationInSeconds { get; set; }

  protected DVD() : base() { }
  public DVD(string isbn, string title, string author, int durationInSeconds, int publishedYear, string description = "", string? imageUrl = null)
      : base(isbn, title, author, publishedYear, description, imageUrl)
  {
    DurationInSeconds = durationInSeconds;
  }
}