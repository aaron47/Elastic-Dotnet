namespace elastic_dotnet.Config;

public class JwtOptions
{
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public int? ExpiresIn { get; set; }
}