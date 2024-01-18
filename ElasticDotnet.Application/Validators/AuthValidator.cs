using ElasticDotnet.Domain.Models;
using FluentValidation;

namespace ElasticDotnet.Application.Validators;

public class AuthValidator : AbstractValidator<AuthDTO>
{
	public AuthValidator()
	{
		RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("EmailRequired");
		RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
		RuleFor(x => x.Password).MinimumLength(6).WithMessage("Password must be at least 6 characters long");
	}
}