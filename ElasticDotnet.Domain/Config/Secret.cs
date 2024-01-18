namespace ElasticDotnet.Domain.Config;

public sealed class Secret
{
    public required string JwtSecret { get; set; }
}