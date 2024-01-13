namespace elastic_dotnet.Config;

public class JwtOptions
{
	public required string Secret { get; set; }
	public required string Issuer { get; set; }
	public required string Audience { get; set; }
	public required int ExpiresIn { get; set; }
}