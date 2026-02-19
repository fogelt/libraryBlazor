using System.ComponentModel.DataAnnotations;

namespace Library.Core.DTOs;

public class AddMemberDto
{
  [Required]
  public string MemberId { get; set; } = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();

  [Required(ErrorMessage = "Name is required")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters")]
  public string Name { get; set; } = "";

  [Required(ErrorMessage = "Email address is required")]
  [EmailAddress(ErrorMessage = "Invalid email format")]
  public string Email { get; set; } = "";

  [Required]
  public DateTime MembershipDate { get; set; } = DateTime.Now;

  [Range(0, 100, ErrorMessage = "Score must be between 0 and 100")]
  public int ActiveScore { get; set; } = 0;
}