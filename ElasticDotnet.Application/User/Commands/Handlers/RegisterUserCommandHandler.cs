using ElasticDotnet.Domain.Interfaces;
using ElasticDotnet.Domain.Models;
using FluentValidation;
using MediatR;

namespace ElasticDotnet.Application.User.Commands.Handlers;

public sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ServiceResult<string>>
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<AuthDTO> _authValidator;

    public RegisterUserCommandHandler(IUserRepository userRepository, ISender sender, IValidator<AuthDTO> authValidator)
    {
        _userRepository = userRepository;
        _authValidator = authValidator;
    }


    public async Task<ServiceResult<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _authValidator.ValidateAsync(request.AuthDto, cancellationToken);
        var dbUser = await _userRepository.FindByEmail(request.AuthDto.Email);

        if (!result.IsValid)
        {
            return ServiceResult<string>.FailureResult(result.Errors.Select(e => e.ErrorMessage));
        }

        if (dbUser is not null)
        {
            var errors = new[] { "Email already in use." };
            return ServiceResult<string>.FailureResult(errors);
        }


        var user = new Domain.Entities.User
        {
            Email = request.AuthDto.Email,
            Password = HashPassword(request.AuthDto.Password)
        };

        await _userRepository.RegisterUser(user);
        return ServiceResult<string>.SuccessResult(request.AuthDto.Email, "User registered successfully.");
    }

    private static string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);
}