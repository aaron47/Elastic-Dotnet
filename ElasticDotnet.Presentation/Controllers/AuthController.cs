using ElasticDotnet.Application.User.Commands;
using ElasticDotnet.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ElasticDotnet.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthDTO authDto)
    {
        var registerUserCommand = new RegisterUserCommand(authDto);

        var result = await _sender.Send(registerUserCommand);

        if (!result.Success)
        {
            return BadRequest(new { errors = result.Errors });
        }

        return Ok(new { message = result.Message, data = result.Data });
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthDTO authDto)
    {
        var loginUserCommand = new LoginUserCommand(authDto);

        var result = await _sender.Send(loginUserCommand);

        if (!result.Success)
        {
            return BadRequest(new { errors = result.Errors });
        }

        return Ok(new { message = result.Message, data = result.Data });
    }
}