using System.ComponentModel.DataAnnotations;

namespace elastic_dotnet.Models;

public class User
{
	[Key]
	public int Id { get; set; }

	[Required]
	[StringLength(255)]
	public required string Email { get; set; }

	[Required]
	[StringLength(255)]
	public required string Password { get; set; }
}