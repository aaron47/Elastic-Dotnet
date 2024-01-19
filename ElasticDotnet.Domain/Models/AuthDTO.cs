namespace ElasticDotnet.Domain.Models;

public sealed class AuthDTO
{
	public required string Email { get; set; }
	public required string Password { get; set; }
}