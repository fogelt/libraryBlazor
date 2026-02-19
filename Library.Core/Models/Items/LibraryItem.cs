using System.ComponentModel.DataAnnotations;
using Library.Core.Interfaces;

namespace Library.Core.Models.Items;

public abstract class LibraryItem : ISearchable
{
  [Key]
  public string ISBN { get; set; } = null!;

  [Required]
  [StringLength(200)]
  public string Title { get; set; } = null!;

  [Required]
  public string Author { get; set; } = null!;

  [Range(0, 2100)]
  public int PublishedYear { get; set; }

  public bool IsAvailable { get; set; } = true;

  [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters.")]
  public string Description { get; set; } = "No description available.";

  [Url(ErrorMessage = "Please provide a valid image URL.")]
  public string? ImageUrl { get; set; }

  protected LibraryItem() { }

  protected LibraryItem(string isbn, string title, string author, int year, string description = "", string? imageUrl = null)
  {
    ISBN = isbn;
    Title = title;
    Author = author;
    PublishedYear = year;
    Description = string.IsNullOrWhiteSpace(description) ? $"A wonderful {GetType().Name.ToLower()} by {author}." : description;
    ImageUrl = imageUrl;
  }

  public virtual bool Matches(string searchTerm) =>
      string.IsNullOrWhiteSpace(searchTerm) ||
      ISBN.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
      Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
      Author.Contains(searchTerm, StringComparison.OrdinalIgnoreCase);
}