using System.ComponentModel.DataAnnotations;

namespace ElasticDotnet.Domain.Entities;

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