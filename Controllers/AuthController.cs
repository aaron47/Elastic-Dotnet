using elastic_dotnet.Models;
using elastic_dotnet.Services;
using Microsoft.AspNetCore.Mvc;

namespace elastic_dotnet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IUsersService usersService) : ControllerBase
{
	[HttpPost("register")]
	public async Task<IActionResult> Register([FromBody] AuthDTO authDto)
	{
		var result = await usersService.Register(authDto);
		if (!result.Success)
		{
			return BadRequest(new { errors = result.Errors });
		}

		return Ok(new { message = result.Message, data = result.Data });
	}


	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] AuthDTO authDto)
	{
		var result = await usersService.Login(authDto);

		if (!result.Success)
		{
			return BadRequest(new { errors = result.Errors });
		}

		return Ok(new { message = result.Message, data = result.Data });
	}
}