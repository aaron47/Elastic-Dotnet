using elastic_dotnet.Models;

namespace elastic_dotnet.Services;

public interface IJwtService
{
	public string GenerateToken(AuthDTO authDto);
}