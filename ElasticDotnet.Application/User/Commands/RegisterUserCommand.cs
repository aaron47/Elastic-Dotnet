using ElasticDotnet.Domain.Models;
using MediatR;

namespace ElasticDotnet.Application.User.Commands;

public record RegisterUserCommand(AuthDTO AuthDto) : IRequest<ServiceResult<string>>;