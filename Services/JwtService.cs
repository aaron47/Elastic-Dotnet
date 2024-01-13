using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using elastic_dotnet.Config;
using elastic_dotnet.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace elastic_dotnet.Services;


public class JwtService(IOptions<JwtOptions> jwtOptions) : IJwtService
{
	private readonly JwtOptions _jwtOptions = jwtOptions.Value;

	public string GenerateToken(AuthDTO authDto)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var secret = Encoding.UTF8.GetBytes(_jwtOptions.Secret);

		var claims = new List<Claim>
		{
			new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			new(JwtRegisteredClaimNames.Sub, authDto.Email),
			new(JwtRegisteredClaimNames.Email, authDto.Email)
		};

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(claims),
			Expires = DateTime.UtcNow.Add(TimeSpan.FromHours(_jwtOptions.ExpiresIn)),
			Issuer = _jwtOptions.Issuer,
			Audience = _jwtOptions.Audience,
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256)
		};


		var token = tokenHandler.CreateToken(tokenDescriptor);
		var jwt = tokenHandler.WriteToken(token);

		return jwt;
	}
}