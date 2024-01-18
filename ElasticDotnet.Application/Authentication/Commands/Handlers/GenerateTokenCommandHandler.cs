using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ElasticDotnet.Domain.Config;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ElasticDotnet.Application.Authentication.Commands.Handlers;

public sealed class GenerateTokenCommandHandler : IRequestHandler<GenerateTokenCommand, string>
{
    private readonly JwtOptions _jwtOptions;
    private readonly Secret _secret;

    public GenerateTokenCommandHandler(IOptions<JwtOptions> jwtOptions, IOptions<Secret> jwtSecret)
    {
        _jwtOptions = jwtOptions.Value;
        _secret = jwtSecret.Value;
    }

    public Task<string> Handle(GenerateTokenCommand request, CancellationToken cancellationToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var secret = Encoding.UTF8.GetBytes(_secret.JwtSecret);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, request.AuthDto.Email),
            new(JwtRegisteredClaimNames.Email, request.AuthDto.Email)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(TimeSpan.FromHours((double)_jwtOptions.ExpiresIn!)),
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256)
        };


        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);

        return Task.FromResult(jwt);
    }
}