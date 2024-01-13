namespace elastic_dotnet.Models;

public class AuthDTO
{
	public required string Email { get; set; }
	public required string Password { get; set; }

	public User ToUser() => new() { Email = Email, Password = Password };
}