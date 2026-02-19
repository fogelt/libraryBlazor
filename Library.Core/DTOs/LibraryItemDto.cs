using System.ComponentModel.DataAnnotations;

namespace Library.Core.DTOs;

public class LibraryItemDto
{
  [Required(ErrorMessage = "ISBN is required")]
  public string ISBN { get; set; } = "";

  [Required(ErrorMessage = "Title is required")]
  public string Title { get; set; } = "";

  [Required(ErrorMessage = "Author is required")]
  public string Author { get; set; } = "";

  [Range(0, 2100, ErrorMessage = "Enter a valid year")]
  public int PublishedYear { get; set; } = DateTime.Now.Year;

  [Required(ErrorMessage = "Description is required")]
  public string Description { get; set; } = "";

  [Url(ErrorMessage = "Please provide a valid image URL.")]
  public string? ImageUrl { get; set; }

  // Maps to Pages, Duration, or Issue #
  public int SpecialMetric { get; set; } = 0;
}