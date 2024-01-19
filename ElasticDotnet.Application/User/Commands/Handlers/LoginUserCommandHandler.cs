using System.Runtime.CompilerServices;
using ElasticDotnet.Application.Authentication.Commands;
using ElasticDotnet.Domain.Interfaces;
using MediatR;

namespace ElasticDotnet.Application.User.Commands.Handlers;

internal sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, ServiceResult<string>>
{
    private readonly IUserRepository _userRepository;
    private readonly ISender _sender;

    public LoginUserCommandHandler(IUserRepository userRepository, ISender sender)
    {
        _userRepository = userRepository;
        _sender = sender;
    }

    public async Task<ServiceResult<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByEmail(request.AuthDto.Email);

        if (user is null)
        {
            var errors = new[] { "User does not exist." };
            return ServiceResult<string>.FailureResult(errors);
        }

        var generationTokenCommand = new GenerateTokenCommand(request.AuthDto);
        var token = await _sender.Send(generationTokenCommand, cancellationToken);

        return ServiceResult<string>.SuccessResult(token, "Logged in successfully");
    }
}