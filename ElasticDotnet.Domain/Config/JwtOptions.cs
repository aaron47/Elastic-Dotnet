namespace ElasticDotnet.Domain.Config;

public sealed class JwtOptions
{
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public int? ExpiresIn { get; set; }
}